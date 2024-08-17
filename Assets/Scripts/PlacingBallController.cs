using UnityEngine;
using UnityEngine.InputSystem;

public class PlacingBallController : MonoBehaviour
{
    [SerializeField] private GameObject defaultPlanetPrefab;
    private GameObject _planetInstance;
    private bool _isControllerActive;

    public void EnableControls()
    {
        _isControllerActive = true;
        _planetInstance = null;
        ChangePlanet(defaultPlanetPrefab);
    }

    public void DisableControls()
    {
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
        GameManager.instance.SetCurrentPlanet(_planetInstance.GetComponent<Planet>());
    }

    private void FollowMouse()
    {
        if (!_isControllerActive)
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