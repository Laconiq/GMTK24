using TMPro;
using UnityEngine;

public class PlanetInfos : MonoBehaviour
{
    [SerializeField] private TMP_Text planetName;
    [SerializeField] private TMP_Text distanceTravelled;
    [SerializeField] private GameObject planetInfos;

    private Celestial _currentCelestial;
    
    public void ShowPlanetInfos(bool b)
    {
        planetInfos.SetActive(b);
    }
    
    public void SetPlanet(Celestial celestial)
    {
        _currentCelestial = celestial;
        planetName.text = celestial.GetPlanetName();
        distanceTravelled.text = celestial.GetTotalDistanceTraveled().ToString("F2");
        InvokeRepeating(nameof(UpdatePlanetInfos), 0f, 1f);
    }
    
    private void UpdatePlanetInfos()
    {
        distanceTravelled.text = _currentCelestial.GetTotalDistanceTraveled().ToString("F2");
    }
}
