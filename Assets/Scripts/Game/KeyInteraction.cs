using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInteraction : InteractiveObject
{
    public override void Action()
    {
        playerController.CollectObject(gameObject);
    }
}
