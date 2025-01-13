using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float lerpSpeed = 3.0f;

    private Vector3 offset;

    private Vector3 targetPos;
    public delegate void OnGetPlayer(Transform transform);
    public static OnGetPlayer onGetPlayer;

    public void Awake()
    {
        onGetPlayer += setUpPlayer;
    }

    public void OnDestroy()
    {
        onGetPlayer -= setUpPlayer;
    }

    private void setUpPlayer(Transform playerTransform)
    {
        target = playerTransform;
        if (target == null) return;

        offset = new Vector3(0, 0, -10);
    }

    private void Update()
    {
        if (target == null) return;

        targetPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.deltaTime);
    }

}
