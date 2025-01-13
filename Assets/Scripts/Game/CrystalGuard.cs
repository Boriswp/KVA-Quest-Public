using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalGuard : InteractiveObject
{
    public EnemyController[] guards;
    private bool canGet;
    public override void Action()
    {
        canGet = true;
        foreach (var item in guards)
        {
            if (!item.isDeath)
            {
                canGet = false;
                break;
            }
        }
        if (canGet)
        {
            BaseDirector.onRaiseObject(GetComponent<Collectable>());
            Destroy(gameObject);
        }
    }
}
