using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootOverlaping : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        Destroy(other);
    }
}
