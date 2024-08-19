using UnityEngine;

public class PlacingBallController : MonoBehaviour
{
    [SerializeField] private GameObject defaultPlanetPrefab;
    private LayerMask _groundLayerMask;
    private GameObject _planetInstance;
    public GameObject GetPlanetInstance() { return _planetInstance; }
    private bool _isControllerActive;
    private GameObject _lastPlanetPrefab;
    private MouseController _mouseController;

    private void Awake()
    {
        _mouseController = FindObjectOfType<MouseController>();
        _groundLayerMask = LayerMask.GetMask("Plane");
    }

    public void EnableControls()
    {
        _planetInstance = null;
        ChangePlanet(_lastPlanetPrefab ? _lastPlanetPrefab : defaultPlanetPrefab);
        _isControllerActive = true;
    }

    public void DisableControls()
    {
        _planetInstance = null;
        _isControllerActive = false;
    }

    private void Update()
    {
        FollowMouse();
    }

    public void ChangePlanet(GameObject planetPrefab)
    {
        if (_planetInstance is not null)
            Destroy(_planetInstance);
        _planetInstance = Instantiate(planetPrefab, Vector3.zero, Quaternion.identity);
        GameManager.Instance.SetCurrentPlanet(_planetInstance.GetComponent<Planet>());
        _lastPlanetPrefab = planetPrefab;
        FollowMouse();
    }

    private void FollowMouse()
    {
        if (!_isControllerActive)
            return;

        if (_planetInstance is null)
            return;

        if (!Physics.Raycast(_mouseController.GetMouseRay(), out var hit, Mathf.Infinity, _groundLayerMask))
            return;

        _planetInstance.transform.position = new Vector3(hit.point.x, 0, hit.point.z);
    }
}