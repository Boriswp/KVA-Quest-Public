using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineDirector : MonoBehaviour
{
    private PlayableDirector director;
    public GameObject dialog;
    public bool isNeedShowDialog = false;
    public bool isPlayOnAwake = false;
    private GameObject[] playerUiObjects;
    public AudioClip clipOnCutScene;
    public AudioClip clipAfterCutScene;

    void Start()
    {
        director = GetComponent<PlayableDirector>();
        director.played += onDirectorPlay;
        director.stopped += onDirectorStopped;
        if (isPlayOnAwake)
        {
            StartCatScene();
        }
    }

    private void onDirectorStopped(PlayableDirector obj)
    {
        if (isNeedShowDialog)
        {
            dialog.SetActive(true);
        }
        else
        {
            PlayerController.isBlockedControl = false;
            PlayerUIController.Instance.ActivateGameObjdect();
        }
        ChangeAudio(clipAfterCutScene);
        gameObject.SetActive(false);
    }

    private void onDirectorPlay(PlayableDirector obj)
    {
        PlayerController.isBlockedControl = true;
        PlayerUIController.Instance.DeactivateGameObjdect();
    }

    public void OnDisable()
    {
        director.played -= onDirectorPlay;
        director.stopped -= onDirectorStopped;
    }

    public void StartCatScene()
    {
        ChangeAudio(clipOnCutScene);
        director.Play();
    }

    private void ChangeAudio(AudioClip clip)
    {
        if (clip == null) return;
        SoundController.musicEvent?.Invoke(clip, true);
    }

}
