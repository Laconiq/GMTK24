using UnityEngine;

public class ChoosePlanetButton : MonoBehaviour
{
    [SerializeField] private GameObject planetPrefab;
    
    public void OnButtonClick()
    {
        if (planetPrefab == null)
            return;
        GameManager.instance.GetPlacingBallController().ChangePlanet(planetPrefab);
    }
}
