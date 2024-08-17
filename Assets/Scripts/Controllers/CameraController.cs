using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Controls _controls;
    private Vector2 _direction;
    private CinemachineVirtualCamera _virtualCamera;
    private bool _isLookingAtPlanet;
    public bool IsLookingAtPlanet() { return _isLookingAtPlanet; }
    
    private Quaternion _defaultRotation;
    private float _defaultFOV;
    
    [SerializeField] private float speed;
    
    public void Initialize()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        
        _controls = new Controls();
        _controls.Camera.Move.performed += ctx => Move(ctx.ReadValue<Vector2>());
        _controls.Camera.Move.canceled += _ => Move(Vector2.zero);
        _controls.Camera.Enable();
        
        _defaultRotation = gameObject.transform.rotation;
        _defaultFOV = _virtualCamera.m_Lens.FieldOfView;
    }
    
    private void Move(Vector2 direction)
    {
        _direction = direction;
    }
    
    private void Update()
    {
        transform.position += new Vector3(_direction.x, 0, _direction.y) * (speed * Time.deltaTime);
    }
    
    public void LookAt(Transform target)
    {
        _virtualCamera.LookAt = target;

        if (target is null)
        {
            gameObject.transform.rotation = _defaultRotation;
            _virtualCamera.m_Lens.FieldOfView = _defaultFOV;
            _isLookingAtPlanet = false;
            GameManager.instance.GetCurrentPlanet().SetPlanetVisibility();
        }
        else
        {
            _virtualCamera.m_Lens.FieldOfView = 30;
            _isLookingAtPlanet = true;
        }
    }
}
