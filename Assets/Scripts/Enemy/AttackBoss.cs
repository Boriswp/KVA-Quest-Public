using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBoss : MonoBehaviour
{
    private BossController enemyController;
    private Animator animatorController;
    public GameObject fireBall;
    private bool canAttack = true;
    private PlayerController player;

    public int maxCount = 3;
    private int currentCount = 0;

    private void Awake()
    {
        enemyController = GetComponentInParent<BossController>();
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
            currentCount++;
            Invoke("Cooldown", enemyController.attackRateInSec);
        }
    }

    private void GetDamage()
    {

        var bullet = Instantiate(fireBall, new Vector2(transform.position.x, transform.position.y), Quaternion.Euler(0, 0, 0)).GetComponent<Weapon>();
        bullet.transform.up = -(player.transform.position - transform.position);
        bullet.Fire(6f, AfterEffects.None, player.transform.position - transform.position);
    }

    private void Cooldown()
    {
        if (currentCount >= maxCount)
        {
            Invoke(nameof(totalCoolDown), 2f);
        }
        else
        {
            canAttack = true;
        }
    }

    public void totalCoolDown()
    {
        currentCount = 0;
        canAttack = true;
    }
}
