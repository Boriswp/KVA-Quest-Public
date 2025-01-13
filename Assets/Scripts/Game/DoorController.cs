using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Animator animator;
    private BoxCollider2D boxCollider;
    public float await = 0.25f;
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
                boxCollider.enabled = !animator.GetBool("isOpen");
            }

        }
        else
        {
            animationFinished = false;
        }
    }
}
