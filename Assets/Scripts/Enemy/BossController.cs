using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    public int health;

    public int enemyScore = 0;

    public float attackSpeed;

    public float usualSpeed;

    public float attackRateInSec;

    public int damage;

    public Transform[] destinations;

    public EnemyPhase phase = EnemyPhase.Walking;

    public Animator animator;

    public NavMeshAgent agent;

    private GameObject player;

    private int currentPathIndex = 0;

    public int destinationToAttack = 0;

    private NavMeshPath path;

    private SpriteRenderer spriteRenderer;

    public GameObject attackHolder;

    public Slider healthSlider;

    private Vector2 direction;

    private Rigidbody2D rb;

    public bool alwaysGoForPlayer = false;

    public bool isDeath = false;

    private bool isPush = false;

    public GameObject fly;

    public bool[] flyProbability;

    private int radius = 2;

    public List<Vector3> pointsAraound;

    public Vector2[] board;

    void Start()
    {
        agent = GetComponentInParent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        flyProbability = new bool[] { false, false, false, false, true, true };
        phase = EnemyPhase.GoForPlayer;
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        healthSlider.maxValue = health;
        healthSlider.value = health;
        path = new();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDeath) return;
        if (collision.CompareTag("TrapWeapon") || collision.CompareTag("PlayerWeapon"))
        {
            var weapon = collision.gameObject.GetComponent<Weapon>();
            if (Helpers.IsKarma(0.45f))
            {
                weapon.Contact();
                pointsAraound.Clear();
                for (int i = 0; i < 4; i++)
                {
                    float angle = i * Mathf.PI * 2 / 4;

                    float x = Mathf.Cos(angle) * radius;
                    float y = Mathf.Sin(angle) * radius;
                    var newPoint = new Vector2(x + player.transform.position.x, y + player.transform.position.y);
                    if (Helpers.IsPointInRectangle(board, newPoint))
                    {
                        if (agent.CalculatePath(newPoint, path))
                        {
                            if (path.status == NavMeshPathStatus.PathPartial || path.status == NavMeshPathStatus.PathInvalid) continue;
                            pointsAraound.Add(newPoint);
                        }
                    }
                }
                if (pointsAraound.Count == 0)
                {
                    transform.position = board[Random.Range(0, board.Length)];
                }
                else
                {
                    transform.position = pointsAraound[Random.Range(0, pointsAraound.Count + 1)];
                }
                return;
            }

            healthSlider.value -= weapon.damage;
            switch (weapon.afterEffects)
            {
                case AfterEffects.Push:
                    Debug.Log("Push");
                    rb.AddForce(-direction * 300f);
                    break;
            }
            weapon.Contact();
            StartCoroutine(Helpers.ChangeColorIfHit(Color.red, 0.2f, spriteRenderer));
            if (healthSlider.value <= 0)
            {
                onDeath();
            }
        }
        if (collision.CompareTag("PlayerWeapon"))
        {
            phase = EnemyPhase.GoForPlayer;
        }

        if (collision.CompareTag("Tongue"))
        {
            rb.AddForce(-direction * 600f);
            isPush = true;
            Invoke(nameof(Pushing), 0.25f);
        }
    }

    private void Pushing()
    {
        isPush = false;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (isDeath) return;
        switch (phase)
        {
            case EnemyPhase.Walking:
                agent.isStopped = false;
                agent.speed = usualSpeed;
                if (destinations.Length == 0) return;
                if ((transform.position - destinations[currentPathIndex].position).magnitude < 1)
                {
                    currentPathIndex++;
                    if (currentPathIndex >= destinations.Length)
                    {
                        currentPathIndex = 0;
                    }
                    if (destinations.Length == 0) return;
                    agent.SetDestination(destinations[currentPathIndex].position);
                }
                break;
            case EnemyPhase.GoForPlayer:
                agent.isStopped = false;
                agent.speed = attackSpeed;

                if (agent.CalculatePath(player.transform.position, path))
                {
                    if (path.status == NavMeshPathStatus.PathPartial || path.status == NavMeshPathStatus.PathInvalid)
                    {
                        if (alwaysGoForPlayer) return;
                        phase = EnemyPhase.Walking;
                        if (destinations.Length == 0) return;
                        agent.SetDestination(destinations[currentPathIndex].position);
                    }
                    else
                    {
                        var offset = 90f;
                        direction = player.transform.position - transform.position;
                        direction.Normalize();
                        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                        attackHolder.transform.rotation = Quaternion.Euler(Vector3.forward * (angle + offset));
                        agent.SetDestination(player.transform.position);
                    }
                }
                else
                {
                    if (alwaysGoForPlayer) return;
                    phase = EnemyPhase.Walking;
                    if (destinations.Length == 0) return;
                    agent.SetDestination(destinations[currentPathIndex].position);
                }
                break;
            case EnemyPhase.Attaking:
                break;
        }
        var normalizedVector = (agent.pathEndPosition - transform.position);
        if (phase == EnemyPhase.GoForPlayer || phase == EnemyPhase.Attaking)
        {
            normalizedVector = (player.transform.position - transform.position);
        }
        animator.SetFloat("Horizontal", normalizedVector.x);
        animator.SetFloat("Vertical", normalizedVector.y);
        animator.SetFloat("Speed", agent.velocity.sqrMagnitude);

        if (rb.linearVelocity.magnitude > 0 && !isPush)
        {
            rb.linearVelocity = Vector2.zero;
        }

    }


    public void onDeath()
    {
        isDeath = true;
        rb.simulated = false;
        int randomIndex = Random.Range(0, flyProbability.Length);
        if (flyProbability[randomIndex])
        {
            Instantiate(fly, transform.position, transform.rotation);
        }
        if (agent.isActiveAndEnabled)
        {
            agent.isStopped = true;
        }

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        GameDirector.onKillBoss?.Invoke(this);
        Destroy(gameObject);
    }
}

