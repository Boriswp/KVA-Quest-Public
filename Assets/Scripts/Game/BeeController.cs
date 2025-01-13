using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeController : MonoBehaviour
{
    public GameObject player;
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {

        transform.position = player.transform.position + Vector3.down;

        animator.SetFloat("Horizontal", PlayerController.movement.x);
        animator.SetFloat("Vertical", PlayerController.movement.y);
    }
}
