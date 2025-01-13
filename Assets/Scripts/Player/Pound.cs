using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pound : MonoBehaviour
{
    public int StaminaRecover;

    private bool isfull = false;
    private void Awake()
    {
        IndicatorsController.onStaminaFull += staminaIsFull;
    }

    private void staminaIsFull(bool isfull)
    {
        this.isfull = isfull;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isfull)
        {
            StartCoroutine(RecoverCoroutine(1.5f, collision));
        }
    }


    private IEnumerator RecoverCoroutine(float waitTime, Collider2D collision)
    {
        while (!isfull)
        {
            yield return new WaitForSeconds(waitTime);
            collision.GetComponent<PlayerController>().RecoverStamina(StaminaRecover);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().canFire = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().canFire = true;
            isfull = false;
            StopAllCoroutines();
        }

    }
}
