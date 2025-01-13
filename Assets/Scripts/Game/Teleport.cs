using Org.BouncyCastle.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public List<SpriteRenderer> runes;
    public GameObject Gravity;
    public GameObject anotherTeleport;
    public float lerpSpeed;

    private Color curColor;
    private Color targetColor;

    private void Awake()
    {
        var teleportObj = GameObject.FindGameObjectsWithTag("Teleport");
        if (anotherTeleport != null) return;
        foreach (GameObject obj in teleportObj)
        {
            if (obj == gameObject) continue;
            anotherTeleport = obj;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        targetColor = new Color(1, 1, 1, 1);
        Gravity.SetActive(true);
        StartCoroutine(TransformCorutine(other.gameObject));
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        targetColor = new Color(1, 1, 1, 0);
        Gravity.SetActive(false);
        StopAllCoroutines();
    }

    private void Update()
    {
        curColor = Color.Lerp(curColor, targetColor, lerpSpeed * Time.deltaTime);

        foreach (var r in runes)
        {
            r.color = curColor;
        }
    }

    IEnumerator TransformCorutine(GameObject objectToTransform)
    {
        yield return new WaitForSeconds(lerpSpeed + 0.5f);
        objectToTransform.transform.position = anotherTeleport.transform.position;
    }
}
