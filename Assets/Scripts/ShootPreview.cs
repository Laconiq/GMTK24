using UnityEngine;

public class ShootPreview : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    private Transform _currentPlanet;
    private float _maxLineLength;
    
    [SerializeField] private Color shortLineColour;
    [SerializeField] private Color longLineColour;
    private void Awake()
    {
        if (lineRenderer.enabled)
            lineRenderer.enabled = false;
        _maxLineLength = FindObjectOfType<CueController>().GetMaxForce();
    }
    
    public void DrawLine(Vector3 endPosition)
    {
        if (!_currentPlanet)
            return;
        
        lineRenderer.SetPosition(0, _currentPlanet.position);
        lineRenderer.SetPosition(1, endPosition);
        
        var distance = Vector3.Distance(_currentPlanet.position, endPosition);
        var newColour = Color.Lerp(shortLineColour, longLineColour, distance / _maxLineLength);
        lineRenderer.material.color = newColour;
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
