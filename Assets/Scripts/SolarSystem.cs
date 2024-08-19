using System.Collections.Generic;
using UnityEngine;

public class SolarSystem : MonoBehaviour
{
    private readonly List<Planet> _planets = new();
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out Planet planet))
            return;
        
        if (!planet.IsPlanetLaunched())
            return;
        AddPlanet(planet);
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out Planet planet))
            return; 
        RemovePlanet(planet);
    }
    
    public int CountPlanetsByType(Planet type)
    {
        var count = 0;
        foreach (var planet in _planets)
        {
            if (planet.GetPlanetName() == type.GetPlanetName())
                count++;
        }
        return count;
    }

    private void AddPlanet(Planet planet)
    {
        _planets.Add(planet);
        foreach (FMODEvents.MusicalCelestialObject MCO in FMODEvents.instance.systemeEnterCelestialList)
        {
            if (MCO.celestialObject.GetPlanetName() == planet.GetComponent<Planet>().GetPlanetName())
            {
                AudioManager.PlayOneShot(MCO.celestialMusic);
            }
        }
    }
    
    public void RemovePlanet(Planet planet)
    {
        _planets.Remove(planet);
    }
}