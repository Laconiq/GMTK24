using UnityEngine;
using UnityEngine.InputSystem;

public class PlacingBallController : MonoBehaviour
{
    [SerializeField] private GameObject planetPrefab;
    private GameObject _planetInstance;
    private Controls _controls;

    public void Initialize()
    {
        _controls = new Controls();
        _controls.Player.LeftClick.performed += OnLeftClickPerformed;
    }
    
    public void EnableControls()
    {
        _controls.Enable();
    }
    
    public void DisableControls()
    {
        _controls.Disable();
    }

    private void OnLeftClickPerformed(InputAction.CallbackContext context)
    {
        PlacePlanet();
    }

    private void PlacePlanet()
    {
        var mousePosition = Mouse.current.position.ReadValue();
        var worldPosition = Camera.main!.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));
        worldPosition.y = 0;
        if (_planetInstance is null)
        {
            _planetInstance = Instantiate(planetPrefab, worldPosition, Quaternion.identity);
            GameManager.instance.SetCurrentPlanet(_planetInstance.GetComponent<Planet>());
        }
        else
        {
            _planetInstance.transform.position = worldPosition;
        }
        GameManager.instance.SetState(GameManager.GameState.Charging);
    }
}