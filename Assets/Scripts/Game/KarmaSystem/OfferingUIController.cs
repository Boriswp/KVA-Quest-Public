using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OfferingUIController : MonoBehaviour
{
    public Texture2D cursorTexture;
    public Texture2D gameTexture;
    public Button button;

    public int strengthCost;
    public int manaCost;
    public int hpCost;

    public int addStrength;
    public int addMana;
    public int addHp;

    public Toggle strengthToggle;
    public Toggle hpToggle;
    public Toggle manaToggle;

    private void OnEnable()
    {
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
        Time.timeScale = 0;
        PlayerController.isBlockedControl = true;
        hpToggle.isOn = false;
        strengthToggle.isOn = false;
        manaToggle.isOn = false;
        strengthToggle.onValueChanged.AddListener(delegate
        {
            setStrength(strengthToggle);
        });
        hpToggle.onValueChanged.AddListener(delegate
        {
            setHP(hpToggle);
        });
        manaToggle.onValueChanged.AddListener(delegate
        {
            setMana(manaToggle);
        });
        button.interactable = false;
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
        Cursor.SetCursor(gameTexture, Vector2.zero, CursorMode.Auto);
        PlayerController.isBlockedControl = false;
    }

    public void setHP(Toggle toggle)
    {
        if (toggle.isOn)
        {
            button.interactable = true;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(delegate
            {
                KarmaController.Instance.BuyHp(hpCost, addHp);
            });
        }
        else
        {
            button.interactable = false;
        }
    }

    public void setMana(Toggle toggle)
    {
        if (toggle.isOn)
        {
            button.interactable = true;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(delegate
            {
                KarmaController.Instance.BuyMana(manaCost, addMana);
            });
        }
        else
        {
            button.interactable = false;
        }
    }

    public void setStrength(Toggle toggle)
    {
        if (toggle.isOn)
        {
            button.interactable = true;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(delegate
            {
                KarmaController.Instance.BuyStrength(strengthCost, addStrength);
            });
        }
        else
        {
            button.interactable = false;
        }
    }

}
