using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractiveObject : MonoBehaviour
{
    public GameObject canvas;
    private bool isActive = false;
    public PlayerController playerController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isActive = true;
            canvas.SetActive(true);
            playerController = collision.GetComponent<PlayerController>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isActive = false;
            canvas.SetActive(false);
            playerController = null;
        }
    }

    void Update()
    {
        if (isActive && Input.GetButton("Action"))
        {
            Action();
        }
    }

    public abstract void Action();
}
