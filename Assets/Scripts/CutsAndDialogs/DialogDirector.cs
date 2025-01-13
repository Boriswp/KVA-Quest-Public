using BerserkPixel.Prata;
using System;
using UnityEngine;

[Serializable]
public struct DialogWithChoices
{
    public string text;
    public GameObject gameObject;
}

public class DialogDirector : MonoBehaviour
{
    private Interaction interaction;
    public GameObject cutScene;
    public AudioClip clipOnDialog;
    public AudioClip clipAfterDialog;
    public DialogWithChoices[] aLotOfChoices;
    private GameObject playerUiObjects;
    public bool isNeedShowCutScene = false;
    public bool isPlayOnAwake = false;
    private bool blocked = false;

    public GameObject gameObjectToCameraCatch;

    private GameObject player;

    void Start()
    {
        interaction = GetComponent<SimpleInteraction>().GetInteraction();
        interaction.Reset();
        playerUiObjects = GameObject.FindGameObjectWithTag("PlayerUI");
        player = GameObject.FindGameObjectWithTag("Player");
        DialogManager.Instance.OnDialogStart += HandleDialogStart;
        DialogManager.Instance.OnDialogChanged += HandleDialogChanged;
        DialogManager.Instance.OnDialogEnds += HandleDialogEnd;
        DialogManager.Instance.OnDialogCancelled += HandleDialogEnd;
        if (gameObjectToCameraCatch != null)
        {
            CameraFollow.onGetPlayer(gameObjectToCameraCatch.transform);
        }
        if (isPlayOnAwake)
        {
            StartOrContinueDialog();
        }
    }

    private void OnDisable()
    {
        DialogManager.Instance.OnDialogStart -= HandleDialogStart;
        DialogManager.Instance.OnDialogChanged -= HandleDialogChanged;
        DialogManager.Instance.OnDialogEnds -= HandleDialogEnd;
        DialogManager.Instance.OnDialogCancelled -= HandleDialogEnd;
    }

    private void HandleDialogChanged(Dialog dialog)
    {
        if (dialog.choices.Count > 1)
        {
            blocked = true;
        }
        else
        {
            blocked = false;
        }
        for (int i = 0; i < aLotOfChoices.Length; i++)
        {
            if (aLotOfChoices[i].text == dialog.text)
            {
                cutScene = aLotOfChoices[i].gameObject;
                isNeedShowCutScene = true;
                break;
            }
        }
    }

    private void HandleDialogStart()
    {
        PlayerController.isBlockedControl = true;
        PlayerUIController.Instance.DeactivateGameObjdect();
        ChangeAudio(clipOnDialog);
    }

    private void HandleDialogEnd()
    {
        if (isNeedShowCutScene)
        {
            cutScene.gameObject.SetActive(true);
        }
        else
        {
            PlayerController.isBlockedControl = false;
            PlayerUIController.Instance.ActivateGameObjdect();
        }
        CameraFollow.onGetPlayer(player.transform);
        ChangeAudio(clipAfterDialog);
        gameObject.SetActive(false);
    }

    private void Update()
    {

        if (Input.GetButtonDown("Action") && !blocked)
        {
            StartOrContinueDialog();
            SoundController.soundEvent?.Invoke(SoundEvent.BUTTONSOUND);
        }
    }

    public void StartOrContinueDialog()
    {
        DialogManager.Instance.Talk(interaction);
    }

    private void ChangeAudio(AudioClip clip)
    {
        if (clip == null) return;
        SoundController.musicEvent?.Invoke(clip, true);
    }
}
