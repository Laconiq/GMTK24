using UnityEngine;

public class Planet : MonoBehaviour
{
    private Sun _sun;
    private Rigidbody _rb;

    private void Start()
    {
        //InitializePlanet(new Vector3(5, 5, 0));
    }

    private void FixedUpdate()
    {
        ApplyGravity();
    }

    private void ApplyGravity()
    {
        if (_sun is null)
            return;

        var directionToSun = _sun.transform.position - transform.position;
        var distanceToSun = directionToSun.magnitude;

        if (distanceToSun > _sun.maxRange)
            return;

        var forceDirection = directionToSun.normalized;
        var forceMagnitude = _sun.gravitationalConstant * _rb.mass / (distanceToSun * distanceToSun);

        var force = forceDirection * forceMagnitude;
        _rb.AddForce(force);
    }

    public void InitializePlanet(Vector3 velocity)
    {
        Debug.Log("Initializing planet");
        _rb = GetComponent<Rigidbody>();
        _sun = FindObjectOfType<Sun>();
        
        _rb.velocity = velocity;
    }
}