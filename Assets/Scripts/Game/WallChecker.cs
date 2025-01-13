using UnityEngine;

public class WallChecker : MonoBehaviour
{
    private Weapon weapon;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyWeapon")) return;
        collision.gameObject.TryGetComponent(out weapon);
        weapon?.Contact();
    }
}