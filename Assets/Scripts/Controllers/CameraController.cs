using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Controls _controls;
    private Vector2 _direction;
    
    [SerializeField] private float speed;
    
    public void Initialize()
    {
        _controls = new Controls();
        _controls.Camera.Move.performed += ctx => Move(ctx.ReadValue<Vector2>());
        _controls.Camera.Move.canceled += _ => Move(Vector2.zero);
        _controls.Camera.Enable();
    }
    
    private void Move(Vector2 direction)
    {
        _direction = direction;
    }
    
    private void Update()
    {
        transform.position += new Vector3(_direction.x, 0, _direction.y) * (speed * Time.deltaTime);
    }
}
