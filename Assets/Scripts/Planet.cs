using UnityEngine;

public class Planet : MonoBehaviour
{
    private Sun _sun;
    private Rigidbody _rb;
    private float _sunMass;
    private bool _isInitialized;

    public void SetVelocity(Vector3 velocity)
    {
        _rb = GetComponent<Rigidbody>();
        _sun = FindObjectOfType<Sun>();
        GetComponent<TrailRenderer>().enabled = true;
        
        _rb.velocity = velocity;
        _sunMass = _sun.GetComponent<Rigidbody>().mass;
        _isInitialized = true;
    }
    
    private void FixedUpdate()
    {
        ApplyGravity();
    }

    private void ApplyGravity()
    {
        if (_sun is null || !_isInitialized)
            return;

        var directionToSun = _sun.transform.position - transform.position;
        var distanceToSun = directionToSun.sqrMagnitude;

        if (distanceToSun > _sun.maxRange)
        {
            Debug.Log("Max range reached");
            return;
        }

        var forceDirection = directionToSun.normalized;
        var force = forceDirection * (_sun.gravitationalConstant * _sunMass / distanceToSun);

        _rb.AddForce(force);
    }
}