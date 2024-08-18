using TMPro;
using UnityEngine;

public class PlanetInfos : MonoBehaviour
{
    [SerializeField] private TMP_Text planetName;
    [SerializeField] private TMP_Text planetDescription;
    [SerializeField] private TMP_Text distanceTravelled;
    [SerializeField] private TMP_Text planetSpeed;
    [SerializeField] private GameObject planetInfos;
    private GameObject _destroyPlanetButton;
    [SerializeField] private DestroyPlanet destroyPlanet;

    private Celestial _currentCelestial;

    private void Awake()
    {
        _destroyPlanetButton = destroyPlanet.gameObject;
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
        if (_currentCelestial is null)
            return;
        planetName.text = "Planet: " + celestial.GetPlanetName();
        planetDescription.text = celestial.GetPlanetDescription();
        _destroyPlanetButton.SetActive(celestial is Planet);
        UpdatePlanetInfos();
    }
    
    private void UpdatePlanetInfos()
    {
        if (_currentCelestial is null)
            return;
        planetSpeed.text = "Speed: " + _currentCelestial.GetPlanetSpeed().ToString("F2") + " km/s";
        distanceTravelled.text = "Traveled distance: " + _currentCelestial.GetTotalDistanceTraveled().ToString("F2") + " km";
        if(!GameManager.Instance.GetCameraController().IsLookingAtPlanet())
            return;
        Invoke(nameof(UpdatePlanetInfos), 1f);
    }
}
