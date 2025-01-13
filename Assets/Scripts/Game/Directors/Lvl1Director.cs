using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LVL1Director : GameDirector
{

    private int crystalCounter = 0;
    public GameObject BossPhase;
    public GameObject EndPhase;

    public void OnEnable()
    {
        onKillBoss += BossKilled;
        onRaiseObject += CollectCrystal;
        KarmaController.Instance.setCrystals(totalObjects, crystalCounter);
    }

    public void OnDisable()
    {
        onKillBoss -= BossKilled;
        onRaiseObject -= CollectCrystal;
    }

    public void CollectCrystal(Collectable obj)
    {
        crystalCounter++;
        KarmaController.Instance.setCrystals(totalObjects, crystalCounter);
        if (crystalCounter == totalObjects)
        {
            KarmaController.Instance.setNewText("Найти открытую дверь");
            BossPhase.SetActive(true);
        }
    }

    public void BossKilled(BossController enemy)
    {
        EndPhase?.SetActive(true);
        GetKey(enemy.transform.position);
        KarmaController.Instance.setNewText("Подбери ключ и прыгай в портал");
    }
}
