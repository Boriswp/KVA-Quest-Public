using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUiController : MonoBehaviour
{
    public Texture2D cursorTexture;
    public AudioClip menu;
    public Transform MainUI;
    public Transform LvlUI;

    private void Awake()
    {
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
        SoundController.musicEvent?.Invoke(menu, true);
        GoToMenu();
    }

    public void LoadGame(string lvlName)
    {
        ButtonSound();
        SceneManager.LoadScene(lvlName);
    }

    public void LoadArena()
    {
        ButtonSound();
        SceneManager.LoadScene("Arena");
    }

    public void GoToMenu() {
        ButtonSound();
        CameraFollow.onGetPlayer?.Invoke(MainUI);
    }

    public void GoToStory()
    {
        ButtonSound();
        CameraFollow.onGetPlayer?.Invoke(LvlUI);
    }

    public void ExitGame()
    {
        ButtonSound();
        Application.Quit();
    }

    public void ButtonSound()
    {
        SoundController.soundEvent.Invoke(SoundEvent.BUTTONSOUND);
    }
}
