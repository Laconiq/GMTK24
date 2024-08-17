using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialBody : MonoBehaviour
{
    public float mass;
    public float radius;
    public Vector3 initialVelocity;
    Vector3 currentVelocity;

    private void Awake()
    {
        currentVelocity = initialVelocity;
    }

    public void UpdateVelocity(CelestialBody[] allBodies, float timeStep)
    {
        foreach (var otherBody in allBodies)
        {
            if (otherBody != this)
            {
                float sqrDst = (otherBody.GetComponent<Rigidbody>().position - GetComponent<Rigidbody>().position).sqrMagnitude;
                Vector3 forceDir = (otherBody.GetComponent<Rigidbody>().position - GetComponent<Rigidbody>().position).normalized;
                Vector3 force = forceDir * Universe.gravitationalConstant * mass * otherBody.mass / sqrDst;
                Vector3 acceleration = force / mass;
                currentVelocity += acceleration * timeStep;
            }
        }
    }

    public void UpdatePosition(float timeStep)
    {
        GetComponent<Rigidbody>().position += currentVelocity * timeStep;
    }
}
