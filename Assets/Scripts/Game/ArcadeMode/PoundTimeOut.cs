using NavMeshPlus.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PoundTimeOut : MonoBehaviour
{
    public GameObject pound;
    public GameObject plight;
    public GameObject godRays;
    public TilemapCollider2D tilemapCollider2D;
    private bool isDisable = false;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isDisable)
        {
            Invoke(nameof(DisablePound), 5f);
        }
    }

    void DisablePound()
    {
        pound.SetActive(false);
        plight.SetActive(false);
        godRays.SetActive(false);
        tilemapCollider2D.enabled = false;
        isDisable = true;
        Invoke(nameof(EnablePound), 10f);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isDisable)
        {
            DisablePound();
        }
    }

    void EnablePound()
    {
        pound.SetActive(true);
        plight.SetActive(true);
        godRays.SetActive(true);
        isDisable = false;
        tilemapCollider2D.enabled = true;
    }
}
