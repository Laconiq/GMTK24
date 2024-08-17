using UnityEngine;

public class Planet : MonoBehaviour
{
    public Sun sun;
    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        InitializePlanet(new Vector3(5, 5, 0));
    }

    private void FixedUpdate()
    {
        ApplyGravity();
    }

    private void ApplyGravity()
    {
        if (sun is null)
            return;

        var directionToSun = sun.transform.position - transform.position;
        var distanceToSun = directionToSun.magnitude;

        if (distanceToSun > sun.maxRange)
            return;

        var velocity = _rb.velocity;
        var currentSpeed = velocity.magnitude;

        var forceDirection = directionToSun.normalized;
        float forceMagnitude;

        if (currentSpeed < sun.gravitationalConstant / distanceToSun)
            forceMagnitude = sun.gravitationalConstant * _rb.mass / (distanceToSun * distanceToSun);
        else
            forceMagnitude = sun.gravitationalConstant * _rb.mass / (distanceToSun * distanceToSun);

        var force = forceDirection * forceMagnitude;
        _rb.AddForce(force);
    }

    private void InitializePlanet(Vector3 velocity)
    {
        _rb.velocity = velocity;
    }
}