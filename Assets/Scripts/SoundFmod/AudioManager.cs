using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    private EventInstance musicEventInstance;

    private EventInstance[] musicEventInstances;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Audio Manager in the scene");
        }
        instance = this;
    }

    private void Start()
    {
        InitializeMusic(FMODEvents.instance.music);
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

    private void InitializeMusic(EventReference[] musicEventReferences)
    {
        foreach (EventReference eventRef in musicEventReferences)
        {
            //musicEventInstances[eventRef.index] = eventRef.
        }
    }

    //Regarder la vidéo à 26:00 pour des sons se basant sur la distance/spatialisation
    //Crossfading music à 39:00
}
