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

    [SerializeField] private float maxForce;
    
    public float GetMaxForce() { return maxForce; }
    
    [SerializeField] private float minForce;
    private ShootPreview _shootPreview;

    public void EnableControls()
    {
        _shootPreview = FindObjectOfType<ShootPreview>();

        if (_shootPreview is null)
            Debug.LogError("ShootPreview is not assigned in the inspector");

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
        _shootPreview.EnableLine();
    }

    private void ChargeShot()
    {
        var currentMousePosition = Mouse.current.position.ReadValue();
        var direction = new Vector3(currentMousePosition.x, 0, currentMousePosition.y) - new Vector3(_initialMousePosition.x, 0, _initialMousePosition.y);

        var distance = direction.magnitude;
        _forceVector = direction.normalized * distance;
        _forceVector *= -forceModifier;
        
        // Clamp the force vector's magnitude between minForce and maxForce
        var clampedMagnitude = Mathf.Clamp(_forceVector.magnitude, minForce, maxForce);
        _forceVector = _forceVector.normalized * clampedMagnitude;

        var endPosition = GameManager.Instance.GetCurrentPlanet().transform.position + _forceVector;
        _shootPreview.DrawLine(endPosition);
    }

    public void ApplyForce()
    {
        if (!_isControllerActive)
            return;
        var ballInstance = GameManager.Instance.GetCurrentPlanet()?.gameObject;
        if (ballInstance is null)
            return;
        var planetScript = ballInstance.GetComponent<Planet>();
        planetScript?.SetVelocity(_forceVector);
        _shootPreview.DisableLine();
        GameManager.Instance.SetState(GameManager.GameState.Shooting);
        AudioManager.instance.PlayOneShot(FMODEvents.instance.CueStrike);
    }
}