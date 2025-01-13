using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    private EnemyController _enemyController;
    private bool canGoForPLayer;

    private void Awake()
    {
        _enemyController = GetComponentInParent<EnemyController>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(CheckCanGoForPlayer(collision.GetComponent<PlayerController>()));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        StopAllCoroutines();

    }

    private IEnumerator CheckCanGoForPlayer(PlayerController player)
    {
        yield return new WaitForSeconds(0.5f);
        if (_enemyController.phase == EnemyPhase.GoForPlayer || _enemyController.phase == EnemyPhase.Attaking) yield break;
        canGoForPLayer = true;
        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, (player.transform.position - transform.position).normalized);
        if (hit.Length > 0)
        {
            for (int i = 0; i < hit.Length; i++)
            {
                if (hit[i].collider.CompareTag("Enemy") || hit[i].collider.name == "Attack" || hit[i].collider.name == "AttackChecker")
                {
                    continue;
                }
                if (hit[i].collider.CompareTag("Player"))
                {
                    break;
                }
                else
                {
                    canGoForPLayer = false;
                }
            }
        }
        if (canGoForPLayer)
        {
            _enemyController.phase = EnemyPhase.GoForPlayer;
        }
    }
}
