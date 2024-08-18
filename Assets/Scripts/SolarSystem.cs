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
    
    private int GetAllPlanetsCount()
    {
        return _planets.Count;
    }
    
    public int CountPlanetsByType(Planet type)
    {
        var count = 0;
        foreach (var planet in _planets)
        {
            if (planet == type)
                count++;
        }
        Debug.Log("Count of " + type.GetPlanetName() + " : " + count);
        return count;
    }

    private void AddPlanet(Planet planet)
    {
        if (_planets.Contains(planet)) 
            return;
        _planets.Add(planet);
        Debug.Log("Planet added: " + planet.GetPlanetName());
    }
    
    public void RemovePlanet(Planet planet)
    {
        if (!_planets.Contains(planet)) 
            return;
        _planets.Remove(planet);
        Debug.Log("Planet removed: " + planet.GetPlanetName());
    }
}