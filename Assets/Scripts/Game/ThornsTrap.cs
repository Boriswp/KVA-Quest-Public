using System.Collections;
using UnityEngine;

public class ThornsTrap : MonoBehaviour
{
    private Animator animator;
    private BoxCollider2D boxCollider;
    public float secToAction = 3f;
    public float secFromAction = 4f;
    private bool animationFinished = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.normalizedTime >= 1.0f && !animator.IsInTransition(0))
        {
            if (!animationFinished)
            {
                animationFinished = true;
                if (animator.GetBool("getDamage"))
                {
                    onDamageEnd();
                    boxCollider.enabled = false;
                }
                else
                {
                    onStayEnd();
                }
            }
        }
        else
        {
            animationFinished = false;
        }
    }

    public void onDamageEnd()
    {
        StartCoroutine(ThornsCorutine(secFromAction, false));
    }

    public void onStayEnd()
    {
        StartCoroutine(ThornsCorutine(secToAction, true));
    }

    IEnumerator ThornsCorutine(float waitTime, bool isAttack)
    {

        yield return new WaitForSeconds(waitTime);
        animator.SetBool("getDamage", isAttack);
        yield return new WaitForSeconds(0.5f);
        boxCollider.enabled = isAttack;

    }
}
