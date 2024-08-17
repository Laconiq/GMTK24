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

    public void InitializePlanet(Vector3 velocity)
    {
        Debug.Log("Initializing planet");
        _rb = GetComponent<Rigidbody>();
        _sun = FindObjectOfType<Sun>();
        
        _rb.velocity = velocity;
    }
}