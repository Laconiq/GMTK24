using MoreMountains.Feedbacks;
using UnityEngine;

public class Planet : MonoBehaviour
{
    private Sun _sun;
    private Rigidbody _rb;
    private float _sunMass;
    private bool _isInitialized;
    private bool _isHidden;
    private PlayerController _playerController;
    private CameraController _cameraController;
    private GameObject _model;
    
    [SerializeField] private MMF_Player growFeedback;
    [SerializeField] private MMF_Player shrinkFeedback;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _sun = FindObjectOfType<Sun>();
        _rb.isKinematic = true;
        _playerController = GameManager.instance.GetPlayerController();
        _cameraController = GameManager.instance.GetCameraController();
        _model = transform.GetChild(0).gameObject;
        growFeedback.PlayFeedbacks();
    }

    public void SetVelocity(Vector3 velocity)
    {
        _rb.isKinematic = false;
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

        var forceDirection = directionToSun.normalized;
        var force = forceDirection * (_sun.gravitationalConstant * (_sunMass * _rb.mass / distanceToSun));

        _rb.AddForce(force);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Celestial") && !_isInitialized && !_isHidden)
        {
            _isHidden = true;
            if (growFeedback.IsPlaying)
                growFeedback.StopFeedbacks();
            shrinkFeedback.PlayFeedbacks();
            _playerController.SetNearestCelestial(other.transform);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Celestial") && !_isInitialized && _isHidden)
        {
            _isHidden = false;
            _playerController.SetNearestCelestial(null);
            if (_cameraController.IsLookingAtPlanet()) 
                return;
            if (shrinkFeedback.IsPlaying)
                shrinkFeedback.StopFeedbacks();
            growFeedback.PlayFeedbacks();
        }
    }

    public void SetPlanetVisibility(bool b)
    {
        _model.SetActive(b);
    }
    
    // Getter and setter
    public bool IsPlanetHidden() { return _isHidden; }
}