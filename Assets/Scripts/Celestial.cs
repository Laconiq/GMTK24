using UnityEngine;

public abstract class Celestial : MonoBehaviour
{
    [Header("Planet Info")]
    [SerializeField] private string celestialName;
    [SerializeField] private string celestialDescription;
    private float _distanceFromBeginning;
    private float _totalDistanceTraveled;
    private Vector3 _lastPosition;

    protected virtual void Awake()
    {
        _lastPosition = transform.position;
    }
    
    protected virtual void FixedUpdate()
    {
        UpdateDistanceTraveled();
    }

    private void UpdateDistanceTraveled()
    {
        var distance = Vector3.Distance(transform.position, _lastPosition);
        _totalDistanceTraveled += distance;
        _lastPosition = transform.position;
    }
    
    // Getters and Setters
    public string GetPlanetName() { return celestialName; }

    public float GetPlanetSpeed()
    {
        return GetComponent<Rigidbody>().velocity.magnitude;
    }
    public virtual float GetTotalDistanceTraveled() { return _totalDistanceTraveled; }

    public string GetPlanetDescription()
    {
        return celestialDescription;
    }
}
