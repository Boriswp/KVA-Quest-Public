using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerChangeAim : MonoBehaviour
{
    public string aimText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            KarmaController.Instance.setNewText(aimText);
        }
    }
}
