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
       // musicCelestialEventInstances[index].setVolume(Mathf);
    }

    //Regarder la vidéo à 26:00 pour des sons se basant sur la distance/spatialisation
    //Crossfading music à 39:00
}
