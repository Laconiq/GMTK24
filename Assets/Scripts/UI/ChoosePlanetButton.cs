using UnityEngine;

public class ChoosePlanetButton : MonoBehaviour
{
    [SerializeField] private GameObject planetPrefab;
    private Animator _animator;
    private static readonly int IsHover = Animator.StringToHash("isHover");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    
    public void OnButtonClick()
    {
        if (planetPrefab == null)
            return;
        GameManager.instance.GetPlacingBallController().ChangePlanet(planetPrefab);
    }
    
    public void HoverEnter()
    {
        _animator.SetBool(IsHover, true);
    }
    
    public void HoverExit()
    {
        _animator.SetBool(IsHover, false);
    }
}
