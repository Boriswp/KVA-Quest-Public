using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public AudioClip buttonSound;
    public AudioClip failClip;
    public AudioClip winClip;
    public static AudioSource[] sources;

    public delegate void SoundControllerEvent(SoundEvent soundEvent);
    public static SoundControllerEvent soundEvent;

    public delegate void MusicControllerEvent(AudioClip audioClip, bool isLoop);
    public static MusicControllerEvent musicEvent;

    public delegate void MusicStopEvent();
    public static MusicStopEvent musicStopEvent;

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Sound");

        if (objs.Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }

        sources = GetComponents<AudioSource>();
        
        soundEvent += OnEventReaction;
        musicEvent += onMusicEventReaction;
    }

    private void Update()
    {
        if (!sources[0].isPlaying) {
              musicStopEvent?.Invoke();
        }
    }

    private void onMusicEventReaction(AudioClip audioClip,bool isLoop) {
        sources[0].clip = audioClip;
        sources[0].loop = isLoop;
        sources[0].Play();
    }

    private void OnEventReaction(SoundEvent soundEvent)
    {
        sources[1].clip = soundEvent switch
        {
            SoundEvent.BUTTONSOUND => buttonSound,
            _ => null,
        };
       
        sources[1].Play();
    }

    private void OnDisable()
    {
        soundEvent -= OnEventReaction;
        musicEvent -= onMusicEventReaction;
    }
}
