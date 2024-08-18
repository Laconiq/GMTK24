using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Controls _controls;
    private Vector2 _direction;
    private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private CinemachineVirtualCamera lookAtCamera;
    private Transform _lookAtCameraTransform;
    private bool _isLookingAtPlanet;
    public bool IsLookingAtPlanet() { return _isLookingAtPlanet; }

    private Quaternion _defaultRotation;
    [SerializeField] private float speed;
    [SerializeField] private float minFOV;
    [SerializeField] private float maxFOV;
    [SerializeField] private float scrollSpeed;

    public void Initialize()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        _lookAtCameraTransform = lookAtCamera.transform;

        _controls = new Controls();
        _controls.Camera.Move.performed += ctx => Move(ctx.ReadValue<Vector2>());
        _controls.Camera.Move.canceled += _ => Move(Vector2.zero);
        _controls.Camera.ScrollWheel.performed += ctx => ScrollWheel(ctx.ReadValue<Vector2>());
        _controls.Camera.ScrollWheel.canceled += _ => ScrollWheel(Vector2.zero);
        _controls.Camera.Enable();

        _defaultRotation = gameObject.transform.rotation;
    }

    private void Move(Vector2 direction)
    {
        _direction = direction;
    }

    private void ScrollWheel(Vector2 scrollDelta)
    {
        if (_isLookingAtPlanet) 
            return;

        var newFOV = _virtualCamera.m_Lens.FieldOfView - scrollDelta.y * scrollSpeed * Time.deltaTime;
        newFOV = Mathf.Clamp(newFOV, minFOV, maxFOV);
        _virtualCamera.m_Lens.FieldOfView = newFOV;
    }

    private void Update()
    {
        transform.position += new Vector3(_direction.x, 0, _direction.y) * (speed * Time.deltaTime);
        _lookAtCameraTransform.position = transform.position;
    }

    public void LookAt(Transform target)
    {
        lookAtCamera.LookAt = target;

        if (target is null)
        {
            gameObject.transform.rotation = _defaultRotation;
            _isLookingAtPlanet = false;

            lookAtCamera.Priority = 9;
            _virtualCamera.Priority = 11;

            GameManager.Instance.GetCurrentPlanet().SetPlanetVisibility(true);
        }
        else
        {
            lookAtCamera.Priority = 11;
            _virtualCamera.Priority = 9;

            GameManager.Instance.GetCurrentPlanet().SetPlanetVisibility(false);

            _isLookingAtPlanet = true;
        }
    }
}