using NavMeshPlus.Extensions;
using Project.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAss : MonoBehaviour
{
    private EnemyController enemyController;
    private Animator animatorController;
    private SpriteRenderer spriteRenderer;
    public GameObject fader;
    private bool canAttack = true;
    private bool isAttack = false;
    private PlayerController player;

    private void Awake()
    {
        enemyController = GetComponentInParent<EnemyController>();
        animatorController = GetComponentInParent<Animator>();
        spriteRenderer = GetComponentInParent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            enemyController.phase = EnemyPhase.Attaking;
            player = collision.GetComponent<PlayerController>();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            enemyController.phase = EnemyPhase.Attaking;
            player = collision.GetComponent<PlayerController>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            enemyController.phase = EnemyPhase.GoForPlayer;
        }
    }

    private void Update()
    {

        if (enemyController.phase == EnemyPhase.Attaking && canAttack)
        {
            canAttack = false;
            isAttack = true;
            enemyController.agent.acceleration = 100;
            enemyController.agent.speed = 50;
            enemyController.agent.SetDestination(player.transform.position);
            animatorController.SetTrigger("isAttack");
            Invoke("GetDamage", 0.65f);
            Invoke("Cooldown", enemyController.attackRateInSec);
        }

        if (isAttack)
        {
            GameObject f = Instantiate(fader, new Vector3(transform.position.x, transform.position.y, transform.position.z + 1), Quaternion.identity) as GameObject;
            SpriteRenderer faderSprite = f.GetComponent<SpriteRenderer>();
            faderSprite.sprite = spriteRenderer.sprite;
            faderSprite.flipX = spriteRenderer.flipX;
        }
    }

    private void GetDamage()
    {
        if ((player.transform.position - enemyController.transform.position).magnitude > 2f) return;
        player.RecieveDamage(enemyController.damage, player.gameObject.transform.position - GetComponentInParent<Transform>().position, AfterEffects.Push);
    }

    private void Cooldown()
    {
        enemyController.agent.acceleration = 10;
        enemyController.agent.speed = enemyController.attackSpeed;
        isAttack = false;
        canAttack = true;
    }

}
