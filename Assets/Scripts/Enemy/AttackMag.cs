using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMag : MonoBehaviour
{
    private EnemyController enemyController;
    private Animator animatorController;
    public GameObject fireBall;
    public Vector3[] vectors;
    private bool canAttack = true;
    private PlayerController player;

    private void Awake()
    {
        enemyController = GetComponentInParent<EnemyController>();
        animatorController = GetComponentInParent<Animator>();
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.GetComponent<PlayerController>();

            if (enemyController.phase == EnemyPhase.Attaking) return;
            var attack = true;
            RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, (player.transform.position - transform.position).normalized);
            if (hit.Length > 0)
            {
                for (int i = 0; i < hit.Length; i++)
                {
                    Debug.Log(hit[i].collider.name);
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
                        attack = false;
                    }
                }
            }
            if (attack)
            {
                enemyController.phase = EnemyPhase.Attaking;
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (enemyController.phase == EnemyPhase.Walking) return;
            enemyController.phase = EnemyPhase.GoForPlayer;
        }
    }

    private void Update()
    {

        if (enemyController.phase == EnemyPhase.Attaking && canAttack)
        {
            enemyController.agent.isStopped = true;
            canAttack = false;
            animatorController.SetTrigger("isAttack");
            Invoke("GetDamage", 0.65f);
            Invoke("Cooldown", enemyController.attackRateInSec);
        }
    }

    private void GetDamage()
    {
        foreach (var vector in vectors)
        {
            var bullet = Instantiate(fireBall, new Vector2(transform.position.x, transform.position.y), transform.rotation).GetComponent<Weapon>();
            bullet.transform.up = -vector;
            bullet.Fire(6f, AfterEffects.None, vector);
        }
    }

    private void Cooldown()
    {
        canAttack = true;
    }

}
