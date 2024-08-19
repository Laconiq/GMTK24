using UnityEngine;
using UnityEngine.InputSystem;

public class PlacingBallController : MonoBehaviour
{
    [SerializeField] private GameObject defaultPlanetPrefab;
    [SerializeField] private LayerMask groundLayerMask;
    private GameObject _planetInstance;
    public GameObject GetPlanetInstance() { return _planetInstance; }
    private bool _isControllerActive;
    private GameObject _lastPlanetPrefab;

    public void EnableControls()
    {
        _planetInstance = null;
        ChangePlanet(_lastPlanetPrefab ? _lastPlanetPrefab : defaultPlanetPrefab);
        _isControllerActive = true;
        _mainCamera = Camera.main;
    }

    public void DisableControls()
    {
        _planetInstance = null;
        _isControllerActive = false;
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
        GameManager.Instance.SetCurrentPlanet(_planetInstance.GetComponent<Planet>());
        _lastPlanetPrefab = planetPrefab;
        FollowMouse();
    }

    private Camera _mainCamera;
    
    private void FollowMouse()
    {
        if (!_isControllerActive)
            return;

        if (_planetInstance is null)
            return;

        var mousePosition = Mouse.current.position.ReadValue();
        
        // Adjusting the mouse position to match the planet's position
        mousePosition.y -= 17;
        mousePosition.x += 15;
        
        var ray = _mainCamera.ScreenPointToRay(mousePosition);
        if (!Physics.Raycast(ray, out var hit, Mathf.Infinity, groundLayerMask))
            return;

        var worldPosition = hit.point;
        worldPosition.y = 0;
        _planetInstance.transform.position = worldPosition;
    }
}