using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static PlayerController;

public class LightChecker : MonoBehaviour
{
    public Light2D lightGame;
    public Light2D lightMap;
    private bool visitedMe = false;
    private bool visitedAnother = false;

    private void OnTriggerExit2D(Collider2D collision)
    {
        var isLocal = collision.GetComponent<PlayerController>()?.isLocalPlayer == true;
        onCountBox?.Invoke(isLocal);
        if (isLocal)
        {
            visitedMe = true;
        }
        else {
            visitedAnother = true;
        }
        lightGame.enabled = true;
        if (visitedMe && visitedAnother)
        {
            lightGame.color = Color.yellow;
            return;
        }
        else if (visitedMe)
        {
            lightGame.color = Color.green;
        }
        else if (visitedAnother) {
            lightGame.color = Color.red;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        lightMap.enabled = true;
    }
}
