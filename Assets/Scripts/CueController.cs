using UnityEngine;
using UnityEngine.InputSystem;

public class CueController : MonoBehaviour
{
    public float forceModifier;
    private bool _isCharging;
    private Vector3 _initialMousePosition;
    private Vector3 _forceVector;
    private Controls _controls;
    private bool _isControllerActive;
    
    [SerializeField] private ShootPreview shootPreview;
    
    public void EnableControls()
    {
        _isControllerActive = true;
        StartCharging();
    }
    
    public void DisableControls()
    {
        _isControllerActive = false;
    }

    private void Update()
    {
        if (!_isControllerActive)
            return;
        ChargeShot();
    }

    private void StartCharging()
    {
        _initialMousePosition = Mouse.current.position.ReadValue();
        shootPreview.EnableLine();
    }

    private void ChargeShot()
    {
        var currentMousePosition = Mouse.current.position.ReadValue();
        var direction = new Vector3(currentMousePosition.x, 0, currentMousePosition.y) - new Vector3(_initialMousePosition.x, 0, _initialMousePosition.y);

        var distance = direction.magnitude;
        _forceVector = direction.normalized * distance;
        _forceVector *= -forceModifier;

        var endPosition = GameManager.instance.GetCurrentPlanet().transform.position + _forceVector;
        shootPreview.DrawLine(endPosition);
    }

    public void ApplyForce()
    {
        if (!_isControllerActive)
            return;
        var ballInstance = GameManager.instance.GetCurrentPlanet()?.gameObject;
        if (ballInstance is null) 
            return;
        var planetScript = ballInstance.GetComponent<Planet>();
        planetScript?.SetVelocity(_forceVector);
        shootPreview.DisableLine();
        GameManager.instance.SetState(GameManager.GameState.Shooting);
    }
}