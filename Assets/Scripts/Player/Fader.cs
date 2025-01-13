using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fader : MonoBehaviour
{
    private SpriteRenderer spriteRender;

    private void Awake()
    {
        spriteRender = GetComponent<SpriteRenderer>();
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float alphaVal = spriteRender.color.a;
        Color tmp = spriteRender.color;

        while (spriteRender.color.a > 0)
        {
            alphaVal -= 0.01f;
            tmp.a = alphaVal;
            spriteRender.color = tmp;

            yield return new WaitForSeconds(0.0005f); // update interval
        }

        Destroy(gameObject);
    }
}
