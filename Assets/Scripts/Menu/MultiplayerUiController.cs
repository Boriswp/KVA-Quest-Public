using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static CameraFollow;
using static GameEndChecker;
using static PlayerController;

public class MultiplayerUiController : MonoBehaviour
{

    public GameObject WinScreen;
    public GameObject LoseScreen;
    public TextMeshProUGUI steps;
    private int localPlayerCounts = 0;
    private int anotherPlayerCounts = 0;
    private MultiplayerStates localStates = MultiplayerStates.PLAY;
    private MultiplayerStates anotherStates = MultiplayerStates.PLAY;
    private Transform localPlayer;
    private Transform anotherPlayer;

    public delegate void OnGetPlayerTransform(Transform transform, bool isLocal);
    public static OnGetPlayerTransform onGetPlayer;

    private void Awake()
    {
        onGameWin += ShowWinScreen;
        onGameLose += ShowEndScreen;
        onCountBox += ChangeCount;
        onGetPlayer += SetUpTransform;
    }

    private void OnDisable()
    {
        onGameWin -= ShowWinScreen;
        onGameLose -= ShowEndScreen;
        onCountBox -= ChangeCount;
        onGetPlayer -= SetUpTransform;
    }

    private void SetUpTransform(Transform player, bool isLocal)
    {
        if (isLocal)
        {
            localPlayer = player;
        }
        else
        {
            anotherPlayer = player;
        }
    }

    private void ChangeCount(bool isLocalPlayer)
    {
        if (isLocalPlayer)
        {
            localPlayerCounts++;
        }
        else
        {
            anotherPlayerCounts++;
        }
        steps.text = "ируш: " + localPlayerCounts.ToString();

    }

    private void ShowWinScreen(bool isLocal)
    {
        if (isLocal)
        {
            localStates = MultiplayerStates.FINISH;
            localPlayer.gameObject.SetActive(false);
        }
        else
        {
            anotherStates = MultiplayerStates.FINISH;
        }
        ChekStatuses(isLocal);
    }


    private void ShowEndScreen(bool isLocal)
    {
        if (isLocal)
        {
            localStates = MultiplayerStates.DIE;
            localPlayer.gameObject.SetActive(false);
        }
        else
        {
            anotherStates = MultiplayerStates.DIE;
        }
        if (localStates == anotherStates)
        {
            Time.timeScale = 0;
            LoseScreen.SetActive(true);
        }
        else
        {
            ChekStatuses(isLocal);
        }
    }
    private void ChekStatuses(bool isLocalPlayer)
    {
        if (localStates == MultiplayerStates.FINISH && anotherStates == MultiplayerStates.DIE)
        {
            Time.timeScale = 0;
            if (isLocalPlayer)
            {
                WinScreen.SetActive(true);
            }
        }
        else if (localStates == MultiplayerStates.DIE && anotherStates == MultiplayerStates.FINISH)
        {
            Time.timeScale = 0;
            if (isLocalPlayer)
            {
                LoseScreen.SetActive(true);

            }
        }
        else if (localStates == MultiplayerStates.FINISH && anotherStates == MultiplayerStates.FINISH)
        {
            Time.timeScale = 0;
            if (anotherPlayerCounts < localPlayerCounts)
            {
                if (isLocalPlayer)
                {
                    LoseScreen.SetActive(true);

                }
            }
            else if (anotherPlayerCounts > localPlayerCounts)
            {
                if (isLocalPlayer)
                {
                    WinScreen.SetActive(true);

                }
            }
            else
            {
                WinScreen.SetActive(true);
            }
        }
        else if (localStates == MultiplayerStates.FINISH || localStates == MultiplayerStates.DIE && anotherStates == MultiplayerStates.PLAY) {
            if (isLocalPlayer) {
                CameraFollow.onGetPlayer.Invoke(anotherPlayer);
            }
        }
    }

    public void LoadMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }
}
