using UnityEngine;
using UnityEngine.InputSystem;

public class PlacingBallController : MonoBehaviour
{
    [SerializeField] private GameObject defaultPlanetPrefab;
    private GameObject _planetInstance;
    private bool _isControllerActive;
    private GameObject _lastPlanetPrefab;

    public void EnableControls()
    {
        _planetInstance = null;
        ChangePlanet(_lastPlanetPrefab != null ? _lastPlanetPrefab : defaultPlanetPrefab);
        _isControllerActive = true;
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

    private void FollowMouse()
    {
        if (!_isControllerActive)
            return;
        
        if (_planetInstance is null)
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