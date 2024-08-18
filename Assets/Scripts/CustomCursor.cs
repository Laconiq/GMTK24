using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    [SerializeField] private Texture2D defaultCursor;
    [SerializeField] private Texture2D clickCursor;
    [SerializeField] private Texture2D hoverCursor;

    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;
    private LayerMask _ignoreLayer;
    private bool _isDefaultCursor;
    private PlacingBallController _placingBallController;

    private void Start()
    {
        _ignoreLayer = LayerMask.GetMask("Ignore Raycast");
        _placingBallController = GameManager.Instance.GetPlacingBallController();
        SetDefaultCursor();
    }

    private void SetDefaultCursor()
    {
        if (_isDefaultCursor)
            return;
        _isDefaultCursor = true;
        Cursor.SetCursor(defaultCursor, hotSpot, cursorMode);
    }

    private void SetHoverCursor()
    {
        if (!_isDefaultCursor)
            return;
        _isDefaultCursor = false;
        Cursor.SetCursor(hoverCursor, hotSpot, cursorMode);
    }

    private void Update()
    {
        var ray = Camera.main!.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out var hit, Mathf.Infinity, ~_ignoreLayer))
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
}