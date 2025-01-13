using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GoldTransactor : InteractiveObject
{
    public int amountOfGold;
    public Sprite barrel;
    public Light2D light2D;
    private SpriteRenderer spriteRenderer;

    public void Awake()
    {
        spriteRenderer = GetComponentInParent<SpriteRenderer>();
    }

    public override void Action()
    {
        light2D.enabled = false;
        spriteRenderer.sprite = barrel;
        KarmaController.Instance.AddMoney(amountOfGold);
        gameObject.SetActive(false);
    }
}
