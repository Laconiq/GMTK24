using UnityEngine;
using UnityEngine.InputSystem;

public class CueController : MonoBehaviour
{
    public GameObject ballPrefab;
    private GameObject ballInstance;
    private bool isPlacingBall = true;
    private bool isCharging = false;
    private Vector3 initialMousePosition;
    private Vector3 forceVector;

    private Controls controls;

    private void Awake()
    {
        controls = new Controls();
        controls.Player.LeftClick.performed += ctx => OnLeftClickPerformed(ctx);
        controls.Player.LeftClick.canceled += ctx => OnLeftClickCanceled(ctx);
        controls.Enable();
    }

    private void OnDestroy()
    {
        controls.Disable();
    }

    private void OnLeftClickPerformed(InputAction.CallbackContext context)
    {
        if (isPlacingBall)
        {
            PlaceBall();
        }
        else
        {
            StartCharging();
        }
    }

    private void OnLeftClickCanceled(InputAction.CallbackContext context)
    {
        if (isCharging)
        {
            isCharging = false;
            ApplyForce();
        }
    }

    private void Update()
    {
        if (isCharging)
        {
            ChargeShot();
        }
    }

    private void PlaceBall()
    {
        var mousePosition = Mouse.current.position.ReadValue();
        var worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));
        worldPosition.y = 0; // Constrain to XZ plane

        if (ballInstance is null)
        {
            ballInstance = Instantiate(ballPrefab, worldPosition, Quaternion.identity);
        }
        else
        {
            ballInstance.transform.position = worldPosition;
        }

        isPlacingBall = false;
    }

    private void StartCharging()
    {
        isCharging = true;
        initialMousePosition = Mouse.current.position.ReadValue();
    }

    private void ChargeShot()
    {
        var currentMousePosition = Mouse.current.position.ReadValue();
        var direction = new Vector3(currentMousePosition.x, 0, currentMousePosition.y) - new Vector3(initialMousePosition.x, 0, initialMousePosition.y);

        var distance = direction.magnitude;
        forceVector = direction.normalized * distance;
        forceVector /= -100;
    }

    private void ApplyForce()
    {
        var planetScript = ballInstance.GetComponent<Planet>();
        planetScript?.InitializePlanet(forceVector);
    }
}