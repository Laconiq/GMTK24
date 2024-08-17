using UnityEngine;

public class Planet : MonoBehaviour
{
    private Sun _sun;
    private Rigidbody _rb;
    private bool _isInitialized;

    public void SetVelocity(Vector3 velocity)
    {
        _rb = GetComponent<Rigidbody>();
        _sun = FindObjectOfType<Sun>();
        GetComponent<TrailRenderer>().enabled = true;
        
        _rb.velocity = velocity;
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
            Debug.Log("Maxrange reached");
            return;
        }

        var forceDirection = directionToSun.normalized;
        var force = forceDirection * _sun.gravitationalConstant * _sun.GetComponent<Rigidbody>().mass / distanceToSun;
        Debug.Log("force"+force);

        _rb.AddForce(force);
    }
}