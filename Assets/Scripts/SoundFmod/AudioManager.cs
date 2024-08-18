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
        }
        Instance = this;

        _musicCelestialEventInstances = new List<EventInstance>();
    }

    private void Start()
    {
        InitializeMusic(FMODEvents.instance.music);
        InitializeMusic(FMODEvents.instance.musicalCelestialList);
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);

        //AudioManager.instance.PlayOneShot(FMODEvents.instance._name, transform.position);
    }
    public void PlayOneShot(EventReference sound)
    {
        RuntimeManager.PlayOneShot(sound);

        //AudioManager.instance.PlayOneShot(FMODEvents.instance._name, transform.position);
    }

    public EventInstance CreateEventInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        return eventInstance;
        //Si on veut faire des loops reprendre la vid�o � 24:00
    }

    private void InitializeMusic(EventReference musicEventReference)
    {
        _musicEventInstance = CreateEventInstance(musicEventReference);
        _musicEventInstance.start();

    }

    private void InitializeMusic(FMODEvents.MusicalCelestialObject[] musicEventReferences)
    {
        for (int i = 0; i < musicEventReferences.Length; i++)
        {
            _musicCelestialEventInstances.Add(CreateEventInstance(musicEventReferences[i].celestialMusic));
            _musicCelestialEventInstances[i].start();
            _musicCelestialEventInstances[i].setVolume(0);
        }
    }

    public void SetMusicVolume(int index, float volume)
    {
        _musicCelestialEventInstances[index].setVolume(volume);
    }

    public void FadeIn(int index, float duration)
    {
        Debug.Log("FadeIn");
        StartCoroutine(FadeVolume(index, 0.0f, 1.0f, duration));
    }

    public void FadeOut(int index, float duration)
    {
        Debug.Log("FadeOut");
        StartCoroutine(FadeVolume(index, 1.0f, 0.0f, duration));
    }

    // Coroutine pour ajuster le volume progressivement
    private IEnumerator FadeVolume(int index, float startVolume, float targetVolume, float duration)
    {
        float elapsedTime = 0f;

        // Initialiser le volume de d�part
        SetMusicVolume(index, startVolume);

        while (elapsedTime < duration)
        {
            // Calculer le volume actuel bas� sur le temps �coul�
            elapsedTime += Time.deltaTime;
            float currentVolume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / duration);

            // Appliquer le volume
            SetMusicVolume(index, currentVolume);

            // Attendre le frame suivant
            yield return null;
        }

        // Assurer que le volume atteint exactement la cible
        SetMusicVolume(index, targetVolume);
    }

    //Regarder la vid�o � 26:00 pour des sons se basant sur la distance/spatialisation
    //Crossfading music � 39:00
}
