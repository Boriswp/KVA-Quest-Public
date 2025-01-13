using UnityEngine;

public class TongueController : MonoBehaviour
{
    private SpriteRenderer _sprite;
    public float speed = 0.75f;
    public float distance = 0f;
    public GameObject player;
    private bool isReturn;
    private Vector3 travelDirection;
    public bool isActive = false;
    private LineRenderer lineRenderer;
    private float movement;
    private CircleCollider2D circleCollider;

    void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        lineRenderer = GetComponent<LineRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Wall") || collision.CompareTag("Mechanism") || collision.CompareTag("Fly")) isReturn = true;
    }

    public void SetUpTarget(Vector3 target)
    {
        travelDirection = target;
        isReturn = false;
        transform.position = player.transform.position;
        isActive = true;
    }


    void Update()
    {
        if (!isActive) return;

        if ((transform.position - player.transform.position).magnitude <= 0.5f && isReturn)
        {
            _sprite.enabled = false;
            isActive = false;
            lineRenderer.enabled = false;
            circleCollider.enabled = false;
            transform.position = player.transform.position;
        }
        else
        {
            if (PlayerController.movement.y < 0 || (PlayerController.movement.x == 0 && PlayerController.movement.y == 0))
            {
                lineRenderer.sortingOrder = 4;
            }
            else
            {
                lineRenderer.sortingOrder = 2;
            }
            _sprite.enabled = true;
            lineRenderer.enabled = true;

            circleCollider.enabled = true;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, player.transform.position);
        }

        if ((transform.position - player.transform.position).magnitude >= distance)
        {
            isReturn = true;
        }

        movement = Time.deltaTime * speed;

        if (isReturn)
        {
            transform.position = Vector3.Lerp(transform.position, player.transform.position, movement);
        }
        else
        {
            transform.Translate(travelDirection.normalized * movement, Space.Self);
        }
    }
}
