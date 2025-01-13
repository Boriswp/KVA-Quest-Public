using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerVisibility : MonoBehaviour
{
    public bool isReady = true;

    private void OnBecameInvisible()
    {
        isReady = true;
    }

    private void OnBecameVisible()
    {
        isReady = false;
    }
}
