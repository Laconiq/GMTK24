using System.Collections.Generic;
using UnityEngine;

public class SolarSystem : MonoBehaviour
{
    private readonly List<Planet> _planets = new List<Planet>();
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out Planet planet))
            return;
        
        if (!planet.IsLaunched)
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
    
    private int CountPlanetsByType(PlanetType type)
    {
        int count = 0;
        foreach (Planet planet in _planets)
        {
            if (planet.GetPlanetType() == type)
                count++;
        }
        return count;
    }
    
    public void AddPlanet(Planet planet)
    {
        if (!_planets.Contains(planet))
            _planets.Add(planet);
    }
    
    public void RemovePlanet(Planet planet)
    {
        if (_planets.Contains(planet))
            _planets.Remove(planet);
    }
}