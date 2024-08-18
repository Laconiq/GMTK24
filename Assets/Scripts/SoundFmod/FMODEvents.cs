using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    //Ref syntaxe
    //[field: Header("name")]
    //[field: SerializeField] public EventReference name { get; private set; }
    [Serializable]
    public class MusicalCelestialObject
    {
        [field: Header("ObjetMusicalCeleste")]
        [field: SerializeField] public EventReference celestialMusic { get; private set; }
        [field: SerializeField] public Planet celestialObject { get; private set; }
    }

    [field: Header("ZoomIn")]
    [field: SerializeField] public EventReference ZoomIn { get; private set; }

    [field: Header("ZoomOut")]
    [field: SerializeField] public EventReference ZoomOut { get; private set; }

    [field: Header("OpenRadialMenu")]
    [field: SerializeField] public EventReference OpenRadialMenu { get; private set; }

    [field: Header("CloseRadialMenu")]
    [field: SerializeField] public EventReference CloseRadialMenu { get; private set; }

    [field: Header("CueStrike")]
    [field: SerializeField] public EventReference CueStrike { get; private set; }

    [field: Header("Music")]
    [field: SerializeField] public EventReference music { get; private set; }

    [field: Header("List Objet Music")]
    [field: SerializeField] public MusicalCelestialObject[] musicalCelestialList { get; private set; }

    [field: Header("List Objet Son Hover")]
    [field: SerializeField] public MusicalCelestialObject[] hoverCelestialList { get; private set; }

    [field: Header("List Objet Son Entrer Systeme")]
    [field: SerializeField] public MusicalCelestialObject[] systemeEnterCelestialList { get; private set; }

    public static FMODEvents instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one FMOD Events in the scene");
        }
        instance = this;
    }
}
