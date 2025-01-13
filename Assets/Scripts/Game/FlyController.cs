using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FlyController : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform[] destinations;
    public List<Vector3> pointsAraound;
    public float radius = 3;
    public int health;
    private int currentPathIndex = 0;
    public float usualSpeed;

    void Start()
    {
        agent = GetComponentInParent<NavMeshAgent>();
        if (destinations.Length == 0)
        {
            for (int i = 0; i < 4; i++)
            {
                float angle = i * Mathf.PI * 2 / 4;

                float x = Mathf.Cos(angle) * radius;
                float y = Mathf.Sin(angle) * radius;

                pointsAraound.Add(new Vector3(x, y, 0) + transform.position);
            }
        }
        else
        {
            for (int i = 0; i < destinations.Length; i++)
            {
                pointsAraound.Add(destinations[i].position);
            }
        }
        agent.SetDestination(pointsAraound[currentPathIndex]);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Tongue"))
        {
            collision.GetComponentInParent<PlayerController>().RecoverHealth(health);
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        agent.isStopped = false;
        agent.speed = usualSpeed;
        if (pointsAraound.Count == 0) return;
        if ((transform.position - pointsAraound[currentPathIndex]).magnitude < 1)
        {
            currentPathIndex++;
            if (currentPathIndex >= pointsAraound.Count)
            {
                currentPathIndex = 0;
            }
            agent.SetDestination(pointsAraound[currentPathIndex]);
        }
    }
}
