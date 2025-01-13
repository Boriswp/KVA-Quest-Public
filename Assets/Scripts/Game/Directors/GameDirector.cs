using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameDirector : BaseDirector
{

    public GameObject key;

    private bool mobsGoForPlayer = false;

    public AudioClip audioClip;

    private AudioClip audioClipBeforeAction;
    private EnemyController[] enemies;

    public void Awake()
    {
        enemies = FindObjectsOfType<EnemyController>();
        StartCoroutine(MusicLoop());
    }

    IEnumerator MusicLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            mobsGoForPlayer = false;
            foreach (var item in enemies)
            {
                if (!item.isDeath && (item.phase == EnemyPhase.GoForPlayer || item.phase == EnemyPhase.Attaking))
                {
                    mobsGoForPlayer = true;
                    try
                    {
                        if (SoundController.sources[0]?.clip != audioClip)
                        {
                            audioClipBeforeAction = SoundController.sources[0]?.clip;
                            SoundController.musicEvent?.Invoke(audioClip, true);
                        }
                        break;
                    }
                    catch
                    {

                    }
                }
            }
            if (!mobsGoForPlayer)
            {
                if (audioClipBeforeAction != null)
                {
                    if (SoundController.sources[0].clip == audioClip)
                    {
                        SoundController.musicEvent?.Invoke(audioClipBeforeAction, true);
                    }
                }
            }
        }
    }

    public void GetKey(Vector3 position)
    {
        Instantiate(key, position, transform.rotation);
    }
}
