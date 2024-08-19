using UnityEngine;

public class MouseController : MonoBehaviour
{
    [SerializeField] private Texture2D defaultCursor;
    [SerializeField] private Texture2D hoverCursor;

    private const CursorMode CursorMode = UnityEngine.CursorMode.Auto;
    private Vector2 _hotSpot;
    private LayerMask _ignoreLayer;
    private bool _isDefaultCursor;
    private PlacingBallController _placingBallController;
    private Camera _mainCamera;

    private void Start()
    {
        if (defaultCursor is null || hoverCursor is null)
            Debug.LogError("Cursor texture not set in MouseController");
        
        _ignoreLayer = LayerMask.GetMask("Ignore Raycast");
        _placingBallController = GameManager.Instance.GetPlacingBallController();
        _mainCamera = Camera.main;
        SetDefaultCursor();
    }

    private void SetDefaultCursor()
    {
        if (_isDefaultCursor)
            return;
        _isDefaultCursor = true;
        _hotSpot = new Vector2(defaultCursor.width / 2, defaultCursor.height / 2);
        Cursor.SetCursor(defaultCursor, _hotSpot, CursorMode);
    }

    private void SetHoverCursor()
    {
        if (!_isDefaultCursor)
            return;
        _isDefaultCursor = false;
        _hotSpot = new Vector2(hoverCursor.width / 2, hoverCursor.height / 2);
        Cursor.SetCursor(hoverCursor, _hotSpot, CursorMode);
    }

    private Ray _mouseRay;
    public Ray GetMouseRay() { return _mouseRay; }
    private void Update()
    {
        UpdateMouseRay();
        
        if (!Physics.Raycast(_mouseRay, out var hit, Mathf.Infinity, ~_ignoreLayer))
        {
            SetDefaultCursor();
            _placingBallController.GetPlanetInstance()?.GetComponent<Planet>().ShowPlanet();
            return;
        }

        var celestial = hit.collider.gameObject.GetComponent<Celestial>();
        var currentPlanet = _placingBallController.GetPlanetInstance();
        if (currentPlanet is null || celestial is null)
        {
            SetDefaultCursor();
            currentPlanet?.GetComponent<Planet>().ShowPlanet();
            return;
        }

        SetHoverCursor();
        currentPlanet.GetComponent<Planet>().HidePlanet(celestial.transform);
    }

    private void UpdateMouseRay()
    {
        _mouseRay = _mainCamera.ScreenPointToRay(Input.mousePosition);
    }
}