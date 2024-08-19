using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    private EventInstance _musicEventInstance;
    private List<EventInstance> _musicCelestialEventInstances;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Found more than one Audio Manager in the scene");
            return;
        }
        Instance = this;
        _musicCelestialEventInstances = new List<EventInstance>();
    }

    private void Start()
    {
        InitializeMusic(FMODEvents.instance.music);
        InitializeMusic(FMODEvents.instance.musicalCelestialList);
    }

    public static void PlayOneShot(EventReference sound)
    {
        RuntimeManager.PlayOneShot(sound);
    }

    private EventInstance CreateEventInstance(EventReference eventReference)
    {
        var eventInstance = RuntimeManager.CreateInstance(eventReference);
        return eventInstance;
    }

    private void InitializeMusic(EventReference musicEventReference)
    {
        _musicEventInstance = CreateEventInstance(musicEventReference);
        _musicEventInstance.start();

    }

    private void InitializeMusic(FMODEvents.MusicalCelestialObject[] musicEventReferences)
    {
        for (var i = 0; i < musicEventReferences.Length; i++)
        {
            _musicCelestialEventInstances.Add(CreateEventInstance(musicEventReferences[i].celestialMusic));
            _musicCelestialEventInstances[i].start();
            _musicCelestialEventInstances[i].setVolume(0);
        }
    }

    private void SetMusicVolume(int index, float volume)
    {
        _musicCelestialEventInstances[index].setVolume(volume);
    }

    public void FadeIn(int index, float duration)
    {
        StartCoroutine(FadeVolume(index, 0.0f, 1.0f, duration));
    }

    public void FadeOut(int index, float duration)
    {
        StartCoroutine(FadeVolume(index, 1.0f, 0.0f, duration));
    }

    private IEnumerator FadeVolume(int index, float startVolume, float targetVolume, float duration)
    {
        var elapsedTime = 0f;
        SetMusicVolume(index, startVolume);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            var currentVolume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / duration);
            SetMusicVolume(index, currentVolume);
            yield return null;
        }
        SetMusicVolume(index, targetVolume);
    }
}
