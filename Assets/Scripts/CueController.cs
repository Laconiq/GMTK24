using UnityEngine;
using UnityEngine.InputSystem;

public class CueController : MonoBehaviour
{
    public float forceModifier = 0;
    private bool _isCharging;
    private Vector3 _initialMousePosition;
    private Vector3 _forceVector;
    private Controls _controls;

    public void Initialize()
    {
        _controls = new Controls();
        _controls.Player.LeftClick.performed += OnLeftClickPerformed;
        _controls.Player.LeftClick.canceled += OnLeftClickCanceled;
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
        StartCharging();
    }

    private void OnLeftClickCanceled(InputAction.CallbackContext context)
    {
        if (!_isCharging) 
            return;
        _isCharging = false;
        ApplyForce();
    }

    private void Update()
    {
        if (_isCharging)
            ChargeShot();
    }

    private void StartCharging()
    {
        _isCharging = true;
        _initialMousePosition = Mouse.current.position.ReadValue();
    }

    private void ChargeShot()
    {
        var currentMousePosition = Mouse.current.position.ReadValue();
        var direction = new Vector3(currentMousePosition.x, 0, currentMousePosition.y) - new Vector3(_initialMousePosition.x, 0, _initialMousePosition.y);

        var distance = direction.magnitude;
        _forceVector = direction.normalized * distance;
        _forceVector *= -forceModifier;
    }

    private void ApplyForce()
    {
        var ballInstance = GameManager.instance.GetCurrentPlanet()?.gameObject;
        if (ballInstance is null) 
            return;
        var planetScript = ballInstance.GetComponent<Planet>();
        planetScript?.SetVelocity(_forceVector);
        GameManager.instance.SetState(GameManager.GameState.Shooting);
    }
}