using UnityEngine;

public class Attack : MonoBehaviour
{
    private EnemyController enemyController;
    private Animator animatorController;
    private bool canAttack = true;
    private PlayerController player;

    private void Awake()
    {
        enemyController = GetComponentInParent<EnemyController>();
        animatorController = GetComponentInParent<Animator>();
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
            animatorController.SetTrigger("isAttack");
            Invoke("GetDamage", 0.65f);
            Invoke("Cooldown", enemyController.attackRateInSec);
        }
    }

    private void GetDamage()
    {
        if ((player.transform.position - enemyController.transform.position).magnitude > 1.45f) return;
        player.RecieveDamage(enemyController.damage, player.gameObject.transform.position - GetComponentInParent<Transform>().position, AfterEffects.Push);
    }

    private void Cooldown()
    {
        canAttack = true;
    }

}
