using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerActivateDialog : MonoBehaviour
{
    public GameObject dialog;

    public bool needShowAgain = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        dialog.SetActive(true);
        if (needShowAgain) return;
        gameObject.SetActive(false);
    }
}
