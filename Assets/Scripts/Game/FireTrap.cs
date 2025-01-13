using System.Collections;
using UnityEngine;
public class FireTrap : MonoBehaviour
{
    private Animator animator;
    private BoxCollider2D boxCollider;
    public float secToAction = 3f;
    public float secFromAction = 4f;
    private bool animationFinished = false;
    public GameObject fireBall;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.normalizedTime >= 1.0f && !animator.IsInTransition(0))
        {
            if (!animationFinished)
            {
                animationFinished = true;
                switch (animator.GetInteger("FireState"))
                {
                    case 0:
                        animator.SetInteger("FireState", 1);
                        StartCoroutine(Fire());
                        break;
                    case 1:
                        StartCoroutine(Wait(2,0.25f));
                        break;
                    case 2:
                        StartCoroutine(Wait(0, 1.5f));
                        break;

                }
            }
        }
        else
        {
            animationFinished = false;
        }
    }

    IEnumerator Wait(int index, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        animator.SetInteger("FireState", index);
    }


    IEnumerator Fire()
    {
        yield return new WaitForSeconds(0.7f);
        Instantiate(fireBall, new Vector2(transform.position.x, transform.position.y - 1.15f), transform.rotation).GetComponent<Weapon>().Fire(3f, AfterEffects.None, Vector3.down);
    }
}
