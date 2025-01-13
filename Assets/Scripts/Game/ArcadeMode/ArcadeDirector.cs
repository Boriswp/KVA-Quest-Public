
using System;
using System.Collections;
using TMPro;
using UnityEngine;

[Serializable]
public struct EnemiesEntity
{
    public int initialCount;
    public GameObject enemyObject;
}

public class ArcadeDirector : BaseDirector
{

    public TMP_Text playerScore;
    public TMP_Text currWaveText;
    public TMP_Text currentEnemies;

    public int secToNewWave;

    public AudioClip[] battleMusics;

    public EnemiesEntity[] enemyWaves;
    private int currWave = 0;
    private int currEnemyCount = 0;
    private SpawnerVisibility[] spawners;


    private string scoreText;
    private string enemyText;
    private string waveText;
    private int score = 0;


    void Awake()
    {
        onKillEnemy += enemyKilled;
        spawners = GetComponentsInChildren<SpawnerVisibility>();
        scoreText = playerScore.text;
        enemyText = currentEnemies.text;
        waveText = currWaveText.text;
        playerScore.text = scoreText + ": " + "0";
        SoundController.musicEvent?.Invoke(battleMusics[0], true);
        StartCoroutine(nextWaveCountdown());
    }

    private void OnDisable()
    {
        onKillEnemy -= enemyKilled;
    }

    private IEnumerator nextWaveCountdown()
    {
        yield return new WaitForSeconds(secToNewWave);
        for (int i = 0; i < enemyWaves.Length; i++)
        {
            var enemies = enemyWaves[i].initialCount + (currWave - i) * (enemyWaves.Length);
            if (enemies < 0) continue;
            currEnemyCount += enemies;
            while (enemies > 0)
            {
                for (int j = 0; j < spawners.Length; j++)
                {
                    if (spawners[j].isReady)
                    {
                        var enemy = enemyWaves[i].enemyObject;
                        enemy.GetComponent<EnemyController>().alwaysGoForPlayer = true;
                        Instantiate(enemy, spawners[j].gameObject.transform.position, spawners[j].gameObject.transform.rotation);
                        enemies--;
                        if (enemies == 0) break;
                    }
                }
            }
        }
        currentEnemies.text = enemyText + ": " + currEnemyCount.ToString();
        currWaveText.text = waveText + ": " + (currWave + 1).ToString();
    }

    private void enemyKilled(EnemyController enemy)
    {
        score += enemy.enemyScore;
        currEnemyCount--;
        currentEnemies.text = enemyText + ": " + currEnemyCount.ToString();
        playerScore.text = scoreText + ": " + score.ToString();
        if (currEnemyCount <= 0)
        {
            currWave++;
            StartCoroutine(nextWaveCountdown());
        }
    }
}
