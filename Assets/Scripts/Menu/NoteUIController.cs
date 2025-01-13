using BerserkPixel.Prata;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NoteUIController : MonoBehaviour
{
    public TMP_Text title;
    public TMP_Text description;
    
    private static NoteUIController _instance;
    public static NoteUIController Instance => _instance;

    public void Awake()
    {

        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            gameObject.SetActive(false);
        }
    }

    public void OnEnable()
    {
        Time.timeScale = 0;
        PlayerController.isBlockedControl = true;
    }

    public void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            gameObject.SetActive(false);
        }
    }

    public void OnDisable()
    {
        Time.timeScale = 1;
        PlayerController.isBlockedControl = false;
    }
}
