using Project.Scripts.Player;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class RingbufferFootSteps : MonoBehaviour
{
    private ParticleSystem system;

    Vector3 lastEmit;

    public float delta = 1;
    public float gap = 0.5f;
    int dir = 1;

    private void Awake()
    {
        system = GetComponentInChildren<ParticleSystem>();
    }

    void Start()
    {
        lastEmit = transform.position;
        StartCoroutine(StepCoroutine());
    }

    private IEnumerator StepCoroutine() {
        while (true) {
            yield return new WaitForSeconds(0.25f);
            if (Vector3.Distance(lastEmit, transform.position) > delta)
            {
                Gizmos.color = Color.green;
                var pos = system.transform.position + (dir * gap * (transform.right * PlayerController.movement.y - PlayerController.movement.x * transform.up));
                dir *= -1;
                ParticleSystem.EmitParams ep = new()
                {
                    position = pos
                };
                var angleInDegrees = Mathf.Atan2(PlayerController.movement.y, PlayerController.movement.x) * 180 / Mathf.PI;
                ep.rotation3D = new Vector3(0, 0, 90 - angleInDegrees);
                system.Emit(ep, 1);
                lastEmit = transform.position;
            }
        }
    }
}
