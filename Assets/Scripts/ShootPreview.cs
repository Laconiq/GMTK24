using UnityEngine;

public class ShootPreview : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    private Transform _currentPlanet;

    private void Awake()
    {
        if (lineRenderer.enabled)
            lineRenderer.enabled = false;
    }

    public void DrawLine(Vector3 endPosition)
    {
        if (!_currentPlanet)
            return;
        lineRenderer.SetPosition(0, _currentPlanet.position);
        lineRenderer.SetPosition(1, endPosition);
    }
    
    public void EnableLine()
    {
        _currentPlanet = GameManager.Instance.GetCurrentPlanet().transform;
        lineRenderer.enabled = true;
    }
    
    public void DisableLine()
    {
        _currentPlanet = null;
        lineRenderer.enabled = false;
    }
}
