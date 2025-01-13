using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDirector : MonoBehaviour
{
    public int totalEnemies = 1;

    public int totalObjects = 1;

    public delegate void OnKillEnemy(EnemyController enemy);
    public static OnKillEnemy onKillEnemy;

    public delegate void OnRaiseObject(Collectable obj);
    public static OnRaiseObject onRaiseObject;

    public delegate void OnKillBoss(BossController enemy);
    public static OnKillBoss onKillBoss;
}
