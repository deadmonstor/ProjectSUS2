using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    
    private static SoundManager instance;
    public static SoundManager Instance => instance;
    [SerializeField] private AudioSource sourcePrefab;
    private Queue<AudioSource> sourceQueue;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private int numberOfSources;
    [SerializeField] private AudioMixer audioMixer;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        
        instance = this;
        DontDestroyOnLoad(gameObject);
        sourceQueue = new Queue<AudioSource>();
        for (int i = 0; i < numberOfSources; i++)
        {
            var source = Instantiate(sourcePrefab);
            sourceQueue.Enqueue(source);
        }
        
        PlayMusic("MenuBGM");
    }

    private void OnEnable()
    {
        Events.onLevelLoaded += LevelLoaded;
    }

    private void OnDisable()
    {
        Events.onLevelLoaded -= LevelLoaded;
    }

    public static void PlaySFX(string clipToPlay, Vector3 position,out AudioSource source)
    {
        SoundManager.Instance.PlaySFXInternal(clipToPlay, position, out source);
    }
    
    private void PlaySFXInternal(string clipToPlay, Vector3 position, out AudioSource source)
    {
        source = null;
        var clip = Resources.Load<AudioClip>($"SFX/{clipToPlay}");
        if (clip == null)
        {
#if UNITY_EDITOR
            Debug.Log($"{clipToPlay} Does Not Exist In Resources");
#endif
            source = null;
            return;
        }

        source = sourceQueue.Dequeue();
        source.clip = clip;
        source.transform.position = position;
        source.Play();
    }
    
    public static void PlaySFX(string clipToPlay, Vector3 position)
    {
        SoundManager.Instance.PlaySFXInternal(clipToPlay, position);
    }
    
    private void PlaySFXInternal(string clipToPlay, Vector3 position)
    {
        var clip = Resources.Load<AudioClip>($"SFX/{clipToPlay}");
        if (clip == null)
        {
#if UNITY_EDITOR
            Debug.Log($"{clipToPlay} Does Not Exist In Resources");
#endif
            return;
        }

        var source = sourceQueue.Dequeue();
        source.clip = clip;
        source.transform.position = position;
        source.Play();
        sourceQueue.Enqueue(source);
    }

    public static void Return(AudioSource source)
    {
        SoundManager.Instance.ReturnInternal(source);
    }

    private void ReturnInternal(AudioSource source)
    {
        if (source != null)
            sourceQueue.Enqueue(source);
    }
    
    public static void PlayMusic(string musicToPlay)
    {
        SoundManager.Instance.PlayMusicInternal(musicToPlay);
    }

    private void PlayMusicInternal(string musicToPlay)
    {
        var clip = Resources.Load<AudioClip>($"Music/{musicToPlay}");
        if (clip == null)
        {
#if UNITY_EDITOR
            Debug.Log($"{musicToPlay} Does Not Exist In Resources");
#endif
            return;
        }
        musicSource.clip = clip;
        musicSource.Play();
    }

    public static void SetVolume(string volToSet, float value)
    {
        SoundManager.Instance.SetVolumeInternal(volToSet, value);
    }

    private void SetVolumeInternal(string volToSet, float value)
    {
        audioMixer.SetFloat(volToSet, value);
    }

    private void LevelLoaded(LevelSO level)
    {
        PlayMusic(level.Music);
    }

}
