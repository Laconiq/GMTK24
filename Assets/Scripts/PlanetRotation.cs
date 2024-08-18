using UnityEngine;

public class PlanetRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 10f;
    
    private void Update()
    {
        transform.rotation *= Quaternion.Euler(0, rotationSpeed * Time.deltaTime, 0);
    }
}
