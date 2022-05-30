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
    [SerializeField] private StringClipDict clipDictionary;
    [SerializeField] private StringClipDict musicDict;
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
        sourceQueue = new Queue<AudioSource>();
        for (int i = 0; i < numberOfSources; i++)
        {
            var source = Instantiate(sourcePrefab);
            sourceQueue.Enqueue(source);
        }
    }

    public static void PlaySFX(string clipToPlay, Vector3 position)
    {
        SoundManager.Instance.PlaySFXInternal(clipToPlay, position);
    }
    
    private void PlaySFXInternal(string clipToPlay, Vector3 position)
    {
        if (!clipDictionary.ContainsKey(clipToPlay))
        {
#if UNITY_EDITOR
            Debug.Log($"{clipToPlay} Does Not Exist In Dictionary");
#endif
            return;
        }

        var source = sourceQueue.Dequeue();
        source.clip = clipDictionary[clipToPlay];
        source.transform.position = position;
        source.Play();
    }

    public static void PlayMusic(string musicToPlay)
    {
        SoundManager.Instance.PlayMusicInternal(musicToPlay);
    }

    private void PlayMusicInternal(string musicToPlay)
    {
        if (!musicDict.ContainsKey(musicToPlay))
        {
#if UNITY_EDITOR
            Debug.Log($"{musicToPlay} Does Not Exist In Dictionary");
#endif
            return;
        }

        musicSource.clip = musicDict[musicToPlay];
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

}
