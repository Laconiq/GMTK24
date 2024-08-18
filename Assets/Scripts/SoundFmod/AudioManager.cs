using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    private EventInstance musicEventInstance;

    private List<EventInstance> musicCelestialEventInstances;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Audio Manager in the scene");
        }
        instance = this;

        musicCelestialEventInstances = new List<EventInstance>();
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
        //Si on veut faire des loops reprendre la vidéo à 24:00
    }

    private void InitializeMusic(EventReference musicEventReference)
    {
        musicEventInstance = CreateEventInstance(musicEventReference);
        musicEventInstance.start();

    }

    private void InitializeMusic(FMODEvents.MusicalCelestialObject[] musicEventReferences)
    {
        for (int i = 0; i < musicEventReferences.Length; i++)
        {
            musicCelestialEventInstances.Add(CreateEventInstance(musicEventReferences[i].celestialMusic));
            musicCelestialEventInstances[i].start();
            musicCelestialEventInstances[i].setVolume(0);
        }
    }

    public void SetMusicVolume(int index, float volume)
    {
        musicCelestialEventInstances[index].setVolume(volume);
    }

    // Fonction pour lancer un fade in
    public void FadeIn(int index, float duration)
    {
        StartCoroutine(FadeVolume(index, 0.0f, 1.0f, duration));
    }

    // Fonction pour lancer un fade out
    public void FadeOut(int index, float duration)
    {
        StartCoroutine(FadeVolume(index, 1.0f, 0.0f, duration));
    }

    // Coroutine pour ajuster le volume progressivement
    private IEnumerator FadeVolume(int index, float startVolume, float targetVolume, float duration)
    {
        float elapsedTime = 0f;

        // Initialiser le volume de départ
        SetMusicVolume(index, startVolume);

        while (elapsedTime < duration)
        {
            // Calculer le volume actuel basé sur le temps écoulé
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

    //Regarder la vidéo à 26:00 pour des sons se basant sur la distance/spatialisation
    //Crossfading music à 39:00
}
