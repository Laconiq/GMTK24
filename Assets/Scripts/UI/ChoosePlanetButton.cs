using UnityEngine;

public class ChoosePlanetButton : MonoBehaviour
{
    [SerializeField] private GameObject planetPrefab;
    private static readonly int IsHover = Animator.StringToHash("isHover");

    public void OnButtonClick()
    {
        if (planetPrefab == null)
            return;
        GameManager.instance.GetPlacingBallController().ChangePlanet(planetPrefab);
    }
}
