using BerserkPixel.Prata;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIController : MonoBehaviour
{
    private static PlayerUIController _instance;
    public static PlayerUIController Instance => _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void ActivateGameObjdect()
    {
        gameObject.SetActive(true);
        KarmaController.Instance?.ActivateObjects();
    }

    public void DeactivateGameObjdect()
    {
        gameObject.SetActive(false);
        KarmaController.Instance?.DeactivateObjects();
    }
}
