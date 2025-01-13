using UnityEngine;

public class GameEndChecker : MonoBehaviour
{
    public delegate void OnGameWin(bool isLocal);
    public static OnGameWin onGameWin;

    public string CollectableNameToEnd = "key";

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            var list = collision.GetComponent<PlayerController>().collectables;

            foreach (var indicator in list)
            {
                if (indicator.GetComponent<Collectable>().name.Contains(CollectableNameToEnd))
                {
                    onGameWin?.Invoke(collision.GetComponent<PlayerController>()?.isLocalPlayer == true);
                }
            }
        }

    }
}
