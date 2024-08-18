using UnityEngine;

public class ChoosePlanetButton : MonoBehaviour
{
    [SerializeField] private GameObject planetPrefab;
    private static readonly int IsHover = Animator.StringToHash("isHover");

    public void OnButtonClick()
    {
        if (planetPrefab == null)
            return;
        GameManager.Instance.GetPlacingBallController().ChangePlanet(planetPrefab);
        foreach (FMODEvents.MusicalCelestialObject MCO in FMODEvents.instance.hoverCelestialList)
        {
            if (MCO.celestialObject.GetPlanetName() == planetPrefab.GetComponent<Planet>().GetPlanetName())
            {
                AudioManager.Instance.PlayOneShot(MCO.celestialMusic);
            }
        }
    }
}
