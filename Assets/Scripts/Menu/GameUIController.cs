using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static GameEndChecker;
using static PlayerController;

public class GameUiController : MonoBehaviour
{
    public GameObject WinScreen;
    public TMP_Text TextCooldown;
    public Image ImageCooldown;
    public GameObject LoseScreen;
    public GameObject PauseMenu;
    public GameObject NoteUI;
    public GameObject StatueUi;
    public Texture2D cursorTexture;
    public Texture2D gameTexture;
    public TextMeshProUGUI steps;
    private int counts = 0;
    private float cooldownTimer = 0.0f;
    private float cooldownTime = 5.0f;

    private void Awake()
    {
        onGameWin += ShowWinScreen;
        onGameLose += ShowEndScreen;
        onCountBox += ChangeCount;
        Cursor.SetCursor(gameTexture, Vector2.zero, CursorMode.Auto);
        TextCooldown.gameObject.SetActive(false);
        ImageCooldown.fillAmount = 0.0f;
    }

    private void OnDisable()
    {
        onGameWin -= ShowWinScreen;
        onGameLose -= ShowEndScreen;
        onCountBox -= ChangeCount;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (NoteUI != null)
            {
                if (NoteUI.activeSelf == true)
                {
                    return;
                }
            }

            if (StatueUi != null)
            {
                if (StatueUi.activeSelf == true)
                {
                    return;
                }
            }

            if (LoseScreen.activeSelf || WinScreen.activeSelf)
                return;

            if (!PauseMenu.activeSelf)
            {
                Time.timeScale = 0;
                PauseMenu.SetActive(true);
                Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
                isBlockedControl = true;
            }
            else
            {
                Continue();
            }
        }
        if (PlayerController.isSprintCoolDown)
        {
            cooldownTimer -= Time.deltaTime;
            TextCooldown.gameObject.SetActive(true);
            TextCooldown.text = Mathf.RoundToInt(cooldownTimer).ToString();
            ImageCooldown.fillAmount = 1 - cooldownTimer / cooldownTime;
        }
        else
        {
            TextCooldown.gameObject.SetActive(false);
            ImageCooldown.fillAmount = 1f;
            cooldownTimer = 5;
        }

    }

    public void Continue()
    {
        PauseMenu.SetActive(false);
        isBlockedControl = false;
        Time.timeScale = 1;
        Cursor.SetCursor(gameTexture, Vector2.zero, CursorMode.Auto);
    }

    public void Reload()
    {
        Time.timeScale = 1;
        isBlockedControl = false;
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    private void ChangeCount(bool isLocalPlayer)
    {
        counts++;
        steps.text = "ируш: " + counts.ToString();

    }

    private void ShowWinScreen(bool isLocalPlayer)
    {
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
        Time.timeScale = 0;
        KarmaController.Instance?.DeactivateObjects();
        WinScreen.SetActive(true);
    }


    private void ShowEndScreen(bool isLocalPlayer)
    {
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
        LoseScreen.SetActive(true);
    }

    public void NextLevel(string nextLevelName)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(nextLevelName);
    }

    public void LoadMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }
}
