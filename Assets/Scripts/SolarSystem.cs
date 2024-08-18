using System.Collections.Generic;
using UnityEngine;

public class SolarSystem : MonoBehaviour
{
    private readonly Dictionary<string, int> _planetCounts = new();

    private void OnTriggerEnter(Collider other)
    {
        var planet = other.GetComponent<Planet>();
        if (planet == null)
            return;

        string planetType = planet.GetPlanetName();
        if (_planetCounts.ContainsKey(planetType))
        {
            _planetCounts[planetType]++;
        }
        else
        {
            _planetCounts[planetType] = 1;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var planet = other.GetComponent<Planet>();
        if (planet == null)
            return;

        string planetType = planet.GetPlanetName();
        if (_planetCounts.ContainsKey(planetType))
        {
            _planetCounts[planetType]--;
            if (_planetCounts[planetType] <= 0)
            {
                _planetCounts.Remove(planetType);
            }
        }
    }

    public int GetPlanetCount<T>(T planetIdentifier)
    {
        string planetType = planetIdentifier switch
        {
            string s => s,
            Planet p => p.GetPlanetName(),
            _ => throw new System.ArgumentException("Invalid type")
        };

        return _planetCounts.ContainsKey(planetType) ? _planetCounts[planetType] : 0;
    }
}