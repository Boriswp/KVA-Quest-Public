using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = Constants.defaultPlayerSpeed;

    private int acceleration = 1;

    public int staminaAttack = 5;

    public int damageWeapon = Constants.defaultPlayerDamage;

    public Rigidbody2D rb;

    public AudioSource audioSource;

    public Animator animator;

    public Light2D playerLight;

    public GameObject bullet;

    public GameObject fader;

    public bool isLocalPlayer;

    public bool isOnline = true;

    private bool LightsOff = false;

    private int newCounter = 0;

    public static Vector2 movement;

    public GameObject foot;

    public delegate void OnGameLose(bool isLocal);
    public static OnGameLose onGameLose;

    public delegate void OnCountBox(bool isLocal);
    public static OnCountBox onCountBox;

    private IndicatorsController indicatorsController;
    private SpriteRenderer spriteRenderer;

    private Weapon weapon;

    public List<GameObject> collectables = new();

    public GameObject used;

    public bool isBlock = false;

    public TongueController tongue;

    public static bool isBlockedControl = false;

    private bool isAttack = false;

    private PlayerWeapon playerWeapon;

    public float coolDownSprint;

    public static bool isSprintCoolDown = false;

    public bool canFire = true;

    private void Awake()
    {
        indicatorsController = GetComponent<IndicatorsController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (!isOnline)
        {
            StartFunc();
        }
    }

    public void StartFunc()
    {
        onCountBox += changeCount;
        MultiplayerUiController.onGetPlayer?.Invoke(this.transform, isLocalPlayer);
        if (isLocalPlayer)
        {
            CameraFollow.onGetPlayer?.Invoke(this.transform);
        }
    }
    public void RecieveDamage(int damage, Vector2 push, AfterEffects afterEffects = AfterEffects.None)
    {
        isAttack = false;
        playerWeapon = null;
        StartCoroutine(Helpers.ChangeColorIfHit(Color.red, 0.2f, spriteRenderer));
        IndicatorsController.onHealthChanged(-damage);
        switch (afterEffects)
        {
            case AfterEffects.Push:
                Debug.Log("Push");
                isBlockedControl = true;
                rb.AddForce(push * 300f);
                StartCoroutine(BlockedWait(0.25f));
                break;
        }


    }

    public void RecoverStamina(int stamina)
    {
        indicatorsController.staminaChange(stamina);
    }

    public void RecoverHealth(int health)
    {
        IndicatorsController.onHealthChanged(health);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("TrapWeapon") || collision.CompareTag("EnemyWeapon") || collision.CompareTag("EnemyLongRange"))
        {
            if (indicatorsController.isInvulnerable) return;
            collision.TryGetComponent(out weapon);
            RecieveDamage(weapon.damage, -movement, weapon.afterEffects);
            weapon.Contact();
        }
    }

    public void CollectObject(GameObject gameObject)
    {
        gameObject.transform.position = used.transform.position;
        gameObject.transform.parent = used.transform;
        collectables.Add(gameObject);
    }

    IEnumerator BlockedWait(float sec)
    {
        yield return new WaitForSeconds(sec);
        rb.linearVelocity = Vector3.zero;
        if (!LightsOff)
        {
            isBlockedControl = false;
        }
    }

    private void OnDisable()
    {
        onCountBox -= changeCount;
        isBlockedControl = false;
        canFire = true;
        isSprintCoolDown = false;
    }

    private void changeCount(bool isLocal)
    {
        if (isLocalPlayer)
        {
            newCounter++;
        }
    }



    void Update()
    {
        if (isBlockedControl)
        {
            audioSource.enabled = false;
            return;
        }

        if (isLocalPlayer)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            if (Input.GetButtonDown("Fire1") && !isAttack && canFire)
            {
                if (indicatorsController.getStamina() >= staminaAttack)
                {
                    isAttack = true;
                    playerWeapon = Instantiate(bullet, transform.position, transform.rotation).GetComponent<PlayerWeapon>();
                    playerWeapon.damage = damageWeapon;
                    playerWeapon.transform.up = MouseInputHandler.MouseWorldPos - transform.position;
                    StartCoroutine(Fire(playerWeapon));
                }
            }

            if (Input.GetButtonDown("Fire2"))
            {
                if (tongue.isActive) return;
                tongue.SetUpTarget(CrosshairMouse.AimDirection);
            }

            if (Input.GetButtonDown("Jump") && movement.magnitude > 0 && !isSprintCoolDown)
            {
                acceleration = 20;
                indicatorsController.isInvulnerable = true;
                isSprintCoolDown = true;
                StartCoroutine(InvulnerableCorutine(0.05f));
                Invoke(nameof(SprintCoolDown), coolDownSprint);
            }

            if (playerWeapon != null && isAttack)
            {
                Vector3 offset = Vector3.zero;

                if (CrosshairMouse.AimDirection.x > 0) offset += Vector3.right / 1.25f;
                else if (CrosshairMouse.AimDirection.x < 0) offset += Vector3.left / 1.25f;

                if (CrosshairMouse.AimDirection.y > 0) offset += Vector3.up / 1.25f;
                else if (CrosshairMouse.AimDirection.y < 0) offset += Vector3.down / 1.25f;

                // Уменьшаем смещение для диагональных направлений
                if (CrosshairMouse.AimDirection.x != 0 && CrosshairMouse.AimDirection.y != 0)
                {
                    offset /= 4;
                }

                playerWeapon.transform.position = transform.position + offset;
            }

            float offsetX = 0f;
            float offsetY = 0f;

            if (movement.x > 0) offsetX = -0.4f;
            else if (movement.x < 0) offsetX = 0.4f;

            if (movement.y > 0) offsetY = -0.65f;
            else if (movement.y < 0) offsetY = 0.65f;

            if (movement.x != 0 && movement.y != 0)
            {
                offsetX *= 0.625f; // 0.4 * 0.625 = 0.25
                offsetY *= 0.615f; // 0.65 * 0.615 ≈ 0.4 or -0.5
            }

            foot.transform.position = new Vector3(transform.position.x + offsetX, transform.position.y + offsetY, 0f);
           
            animator.SetFloat("mHorizontal", CrosshairMouse.AimDirection.x);
            animator.SetFloat("mVertical", CrosshairMouse.AimDirection.y);
            animator.SetFloat("kHorizontal", movement.x);
            animator.SetFloat("kVertical", movement.y);
            animator.SetFloat("Speed", movement.sqrMagnitude);
            animator.SetBool("isAttack", isAttack);

            if (movement.magnitude > 0)
            {
                audioSource.enabled = true;
            }
            else
            {
                audioSource.enabled = false;
            }

            if (indicatorsController.isInvulnerable)
            {
                GameObject f = Instantiate(fader, new Vector3(transform.position.x, transform.position.y, transform.position.z + 1), Quaternion.identity) as GameObject;
                SpriteRenderer faderSprite = f.GetComponent<SpriteRenderer>();
                faderSprite.sprite = spriteRenderer.sprite;
                faderSprite.flipX = spriteRenderer.flipX;
            }
        }
    }

    private void SprintCoolDown()
    {
        isSprintCoolDown = false;
    }

    private void FixedUpdate()
    {
        if (isLocalPlayer && !isBlockedControl && !isAttack)
        {
            rb.MovePosition(rb.position + moveSpeed * acceleration * Time.fixedDeltaTime * movement.normalized);
        }
    }
    IEnumerator Fire(PlayerWeapon weapon)
    {
        yield return new WaitForSeconds(0.5f);
        indicatorsController.staminaChange(-staminaAttack);
        weapon?.Fire(3f, AfterEffects.None, CrosshairMouse.AimDirection);
        isAttack = false;
    }

    private IEnumerator InvulnerableCorutine(float waitTime)
    {

        yield return new WaitForSeconds(waitTime);
        acceleration = 1;
        indicatorsController.isInvulnerable = false;
    }

    public void StartGameEnd()
    {
        LightsOff = true;
        isBlockedControl = true;
        StartCoroutine(LoseCoroutine(0.075f));
    }


    private IEnumerator LoseCoroutine(float waitTime)
    {
        animator.SetBool("isDie", true);
        onGameLose?.Invoke(isLocalPlayer);
        while (LightsOff)
        {
            yield return new WaitForSeconds(waitTime);
            var light = playerLight.intensity;
            var timeScale = Time.timeScale;
            light -= waitTime;
            timeScale -= waitTime * 2;

            if (light <= 0 || timeScale <= 0)
            {
                playerLight.intensity = 0;
                Time.timeScale = 0;
                LightsOff = false;
            }
            else
            {
                playerLight.intensity = light;
                Time.timeScale = timeScale;
            }
        }
    }
}
