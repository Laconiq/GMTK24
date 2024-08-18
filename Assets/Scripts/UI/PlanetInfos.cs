using TMPro;
using UnityEngine;

public class PlanetInfos : MonoBehaviour
{
    [SerializeField] private TMP_Text planetName;
    [SerializeField] private TMP_Text distanceTravelled;
    [SerializeField] private TMP_Text planetSpeed;
    [SerializeField] private GameObject planetInfos;

    private Celestial _currentCelestial;

    private void Awake()
    {
        if (planetInfos.activeSelf)
            planetInfos.SetActive(false);
    }

    public void ShowPlanetInfos(bool b)
    {
        planetInfos.SetActive(b);
    }
    
    public void SetPlanet(Celestial celestial)
    {
        _currentCelestial = celestial;
        planetName.text = "Planet: " + celestial.GetPlanetName();
        UpdatePlanetInfos();
        InvokeRepeating(nameof(UpdatePlanetInfos), 0f, 1f);
    }
    
    private void UpdatePlanetInfos()
    {
        planetSpeed.text = "Speed: " + _currentCelestial.GetPlanetSpeed().ToString("F2") + " km/s";
        distanceTravelled.text = "Traveled distance: " + _currentCelestial.GetTotalDistanceTraveled().ToString("F2") + " km";
    }
}
