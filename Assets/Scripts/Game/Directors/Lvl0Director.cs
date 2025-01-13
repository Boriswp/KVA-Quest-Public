using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lvl0Director : GameDirector
{

    void OnEnable()
    {
        onKillEnemy += EndPhase;
    }

    private void OnDisable()
    {
        onKillEnemy -= EndPhase;
    }

    private void EndPhase(EnemyController enemy)
    {
        totalEnemies--;
        if (totalEnemies <= 0)
        {
            GetKey(enemy.transform.position);
        }
    }

  
}
