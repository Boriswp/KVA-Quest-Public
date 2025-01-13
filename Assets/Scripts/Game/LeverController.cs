using UnityEngine;

public class LeverController : MonoBehaviour
{
    public Animator animatorControllerDoor;
    private Animator animatorControllerLever;
    private bool animationFinished;

    private void Awake()
    {
        animatorControllerLever = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Tongue"))
        {
            Debug.Log("Tongue");
            animatorControllerLever.SetBool("isOpen", !animatorControllerLever.GetBool("isOpen"));
        }
    }

    void Update()
    {
        AnimatorStateInfo stateInfo = animatorControllerLever.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.normalizedTime >= 1.0f && !animatorControllerLever.IsInTransition(0))
        {
            if (!animationFinished)
            {
                animationFinished = true;
                animatorControllerDoor.SetBool("isOpen", !animatorControllerDoor.GetBool("isOpen"));
            }

        }
        else
        {
            animationFinished = false;
        }
    }
}
