using UnityEngine;

public class DialogBubble : MonoBehaviour
{
    [SerializeField] private GameObject bubbleSprite;

    public void ShowBubble()
    {
        if (bubbleSprite != null)
            bubbleSprite.SetActive(true);
    }

    public void HideBubble()
    {
        if (bubbleSprite != null)
            bubbleSprite.SetActive(false);
    }
}
