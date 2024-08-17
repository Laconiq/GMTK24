using UnityEngine;

public class Planet : MonoBehaviour
{
    private Sun _sun;
    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _sun = FindObjectOfType<Sun>();
        
        InitializePlanet(new Vector3(5, 5, 0));
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

        var velocity = _rb.velocity;
        var currentSpeed = velocity.magnitude;

        var forceDirection = directionToSun.normalized;
        float forceMagnitude;

        if (currentSpeed < _sun.gravitationalConstant / distanceToSun)
            forceMagnitude = _sun.gravitationalConstant * _rb.mass / (distanceToSun * distanceToSun);
        else
            forceMagnitude = _sun.gravitationalConstant * _rb.mass / (distanceToSun * distanceToSun);

        var force = forceDirection * forceMagnitude;
        _rb.AddForce(force);
    }

    private void InitializePlanet(Vector3 velocity)
    {
        _rb.velocity = velocity;
    }
}