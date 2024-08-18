using MoreMountains.Feedbacks;
using UnityEngine;

public class Planet : Celestial
{
    private Sun _sun;
    private Rigidbody _rb;
    private float _sunMass;
    private bool _isLaunched;
    private bool _isHidden;
    private PlayerController _playerController;
    private CameraController _cameraController;
    private GameObject _model;
    private bool planetIsInSunRange = false;
    private float rngOrbitalInfluence;
    private int rngIsAfflicted;

    [Header("Feedbacks")]
    [SerializeField] private MMF_Player growFeedback;
    [SerializeField] private MMF_Player shrinkFeedback;

    protected override void Awake()
    {
        base.Awake();
        _rb = GetComponent<Rigidbody>();
        _sun = FindObjectOfType<Sun>();
        _rb.isKinematic = true;
        _playerController = GameManager.instance.GetPlayerController();
        _cameraController = GameManager.instance.GetCameraController();
        _model = transform.GetChild(0).gameObject;
        growFeedback.PlayFeedbacks();

        rngOrbitalInfluence = Random.Range(0f, 1f);
        rngIsAfflicted = Random.Range(0, 2);
        Debug.Log(rngIsAfflicted + "rng");
    }

    public void SetVelocity(Vector3 velocity)
    {
        _rb.isKinematic = false;
        GetComponent<TrailRenderer>().enabled = true;
        
        _rb.velocity = velocity;
        _sunMass = _sun.GetComponent<Rigidbody>().mass;
        _isLaunched = true;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        ApplyGravity();
    }

    private void ApplyGravity()
    {
        if (_sun is null || !_isLaunched)
            return;

        var directionToSun = _sun.transform.position - transform.position;
        var distanceToSun = directionToSun.sqrMagnitude;

        var forceDirection = directionToSun.normalized;
        var force = forceDirection * (_sun.gravitationalConstant * (_sunMass * _rb.mass / distanceToSun));

        #region Orbital Correction
        if (rngIsAfflicted==1)
        {
            var idealOrbitalVelocity = Mathf.Sqrt(_sun.gravitationalConstant * _sunMass / Mathf.Sqrt(distanceToSun));

            var currentVelocity = _rb.velocity;
            Vector3 orbitalVelocityDirection;
            if (Vector3.Dot(Vector3.Cross(directionToSun, Vector3.up), currentVelocity) > 0)
            {
                orbitalVelocityDirection = Vector3.Cross(directionToSun, Vector3.up).normalized;
            }
            else orbitalVelocityDirection = Vector3.Cross(Vector3.up, directionToSun).normalized;

            var currentSpeed = Vector3.Dot(currentVelocity, orbitalVelocityDirection);
            float speedCorrection;
            if (currentSpeed > idealOrbitalVelocity + _sun.orbitalVelocityThreshold || currentSpeed < idealOrbitalVelocity - _sun.orbitalVelocityThreshold)
            {
                speedCorrection = (idealOrbitalVelocity - currentSpeed) * rngOrbitalInfluence * Time.fixedDeltaTime;
            }
            else speedCorrection = (idealOrbitalVelocity - currentSpeed) * Time.fixedDeltaTime;
            _rb.velocity += orbitalVelocityDirection * speedCorrection;
        }
        #endregion

        _rb.AddForce(force);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Sun _) && _isLaunched)
        {
            Die();
            return;
        }
        
        if (!other.CompareTag("Celestial") || _isLaunched || _isHidden) 
            return;

        _isHidden = true;
        if (growFeedback.IsPlaying)
            growFeedback.StopFeedbacks();
        shrinkFeedback.PlayFeedbacks();
        _playerController.SetNearestCelestial(other.transform);
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Celestial") || _isLaunched || !_isHidden) 
            return;
        _isHidden = false;
        _playerController.SetNearestCelestial(null);
        if (_cameraController.IsLookingAtPlanet()) 
            return;
        if (shrinkFeedback.IsPlaying)
            shrinkFeedback.StopFeedbacks();
        growFeedback.PlayFeedbacks();
    }

    public void SetPlanetVisibility(bool b)
    {
        _model.SetActive(b);
    }

    private void Die()
    {
        Destroy(gameObject);
    }
    
    // Getter and setter
    public bool IsPlanetHidden() { return _isHidden; }
}