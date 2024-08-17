using UnityEngine;
using UnityEngine.InputSystem;

public class PlacingBallController : MonoBehaviour
{
    [SerializeField] private GameObject defaultPlanetPrefab;
    private GameObject _planetInstance;
    private Controls _controls;
    private bool _isActive;

    public void Initialize()
    {
        _controls = new Controls();
        _controls.Player.LeftClick.performed += _ => GameManager.instance.SetState(GameManager.GameState.Charging);
    }

    public void EnableControls()
    {
        _controls.Enable();
        _isActive = true;
        ChangePlanet(defaultPlanetPrefab);
    }

    public void DisableControls()
    {
        _controls.Disable();
        _isActive = false;
    }

    private void Update()
    {
        FollowMouse();
    }
    
    public void ChangePlanet(GameObject planetPrefab)
    {
        if (_planetInstance != null)
            Destroy(_planetInstance);
        _planetInstance = Instantiate(planetPrefab, Vector3.zero, Quaternion.identity);
        GameManager.instance.SetCurrentPlanet(_planetInstance.GetComponent<Planet>());
    }

    private void FollowMouse()
    {
        if (!_isActive)
            return;
        var mousePosition = Mouse.current.position.ReadValue();
        var ray = Camera.main!.ScreenPointToRay(mousePosition);
        if (!Physics.Raycast(ray, out var hit)) 
            return;
        var worldPosition = hit.point;
        worldPosition.y = 0;
        _planetInstance.transform.position = worldPosition;
    }
}