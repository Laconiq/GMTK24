using MoreMountains.Feedbacks;
using UnityEngine;

public class Planet : Celestial
{
    private Sun _sun;
    private Rigidbody _rb;
    private float _sunMass;
    private bool _isLaunched;
    public bool IsPlanetLaunched()
    {
        return _isLaunched;
    }
    private bool _isHidden;
    private PlayerController _playerController;
    private CameraController _cameraController;
    private GameObject _model;
    private bool _planetIsInSunRange;
    private float _rngOrbitalInfluence;
    private int _rngIsAfflicted;
    private int _indexAudioManager;

    [Header("Feedbacks")]
    [SerializeField] private MMF_Player growFeedback;
    [SerializeField] private MMF_Player shrinkFeedback;
    
    [Header("Layers")]
    private LayerMask _defaultLayer;
    private LayerMask _ignoreRaycastLayer;

    protected override void Awake()
    {
        base.Awake();
        _rb = GetComponent<Rigidbody>();
        _sun = FindObjectOfType<Sun>();
        _rb.isKinematic = true;
        _playerController = GameManager.Instance.GetPlayerController();
        _cameraController = GameManager.Instance.GetCameraController();
        _model = transform.GetChild(0).gameObject;
        GrowPlanet(true);
        
        _defaultLayer = LayerMask.NameToLayer("Default");
        _ignoreRaycastLayer = LayerMask.NameToLayer("Ignore Raycast");
        gameObject.layer = _ignoreRaycastLayer;

        _rngOrbitalInfluence = Random.Range(0f, 1f);
        _rngIsAfflicted = Random.Range(0, 2);
    }

    public void SetVelocity(Vector3 velocity)
    {
        _rb.isKinematic = false;
        GetComponent<TrailRenderer>().enabled = true;
        
        _rb.velocity = velocity;
        _sunMass = _sun.GetComponent<Rigidbody>().mass;
        _isLaunched = true;
        gameObject.layer = _defaultLayer;
        InvokeRepeating(nameof(CheckDistanceFromSun), 5f, 5f);
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
        if (_rngIsAfflicted==1)
        {
            var idealOrbitalVelocity = Mathf.Sqrt(_sun.gravitationalConstant * _sunMass / Mathf.Sqrt(distanceToSun));

            var currentVelocity = _rb.velocity;
            Vector3 orbitalVelocityDirection;
            if (Vector3.Dot(Vector3.Cross(directionToSun, Vector3.up), currentVelocity) > 0)
                orbitalVelocityDirection = Vector3.Cross(directionToSun, Vector3.up).normalized;
            else 
                orbitalVelocityDirection = Vector3.Cross(Vector3.up, directionToSun).normalized;

            var currentSpeed = Vector3.Dot(currentVelocity, orbitalVelocityDirection);
            float speedCorrection;
            if (currentSpeed > idealOrbitalVelocity + _sun.orbitalVelocityThreshold || currentSpeed < idealOrbitalVelocity - _sun.orbitalVelocityThreshold)
                speedCorrection = (idealOrbitalVelocity - currentSpeed) * _rngOrbitalInfluence * Time.fixedDeltaTime;
            else 
                speedCorrection = (idealOrbitalVelocity - currentSpeed) * Time.fixedDeltaTime;
            _rb.velocity += orbitalVelocityDirection * speedCorrection;
        }
        #endregion

        _rb.AddForce(force);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sun") && _isLaunched)
        {
            for (var i = 0; i < FMODEvents.instance.musicalCelestialList.Length; i++)
            {
                if (FMODEvents.instance.musicalCelestialList[i].celestialObject.GetPlanetName() == this.GetPlanetName())
                {
                    VolumeModifier(i, 1);
                    _indexAudioManager = i;
                }
            }
        }
        if (other.TryGetComponent(out Sun _) && _isLaunched)
            Die();
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Sun") && _isLaunched)
        {
            for (var i = 0; i < FMODEvents.instance.musicalCelestialList.Length; i++)
            {
                if (FMODEvents.instance.musicalCelestialList[i].celestialObject.GetPlanetName() == GetPlanetName())
                {
                    if (FindObjectOfType<SolarSystem>().CountPlanetsByType(this) <= 0)
                    {
                        VolumeModifier(i, 0);
                    }
                }
            }
        }
    }
    
    public void ShowPlanet()
    {
        if (!_isHidden || _cameraController.IsLookingAtPlanet())
            return;
        _isHidden = false;
        _playerController.SetNearestCelestial(null);
        GrowPlanet(true);
    }

    public void HidePlanet(Transform hoveredCelestial)
    {
        if (_isHidden || _cameraController.IsLookingAtPlanet())
            return;
        _isHidden = true;
        GrowPlanet(false);
        _playerController.SetNearestCelestial(hoveredCelestial);
    }

    public void SetPlanetVisibility(bool b)
    {
        _model.SetActive(b);
    }

    public void Die()
    {
        var solarSystem = FindObjectOfType<SolarSystem>();
        solarSystem.RemovePlanet(this);
        if (solarSystem.CountPlanetsByType(this) <= 0)
        {
            VolumeModifier(_indexAudioManager, 0);
        }
        Destroy(gameObject);
    }

    private void VolumeModifier(int index, int volume)
    {
        Debug.Log("VolumeModifier");
        var duration = 2f;

        if (volume == 0)
        {
            AudioManager.Instance.FadeOut(index, duration);
        }
        else AudioManager.Instance.FadeIn(index, duration);
    }

    public void GrowPlanet(bool b)
    {
        if (!b)
        {
            if (growFeedback.IsPlaying)
                growFeedback.StopFeedbacks();
            shrinkFeedback.PlayFeedbacks();
        }
        else
        {
            if (shrinkFeedback.IsPlaying)
                shrinkFeedback.StopFeedbacks();
            growFeedback.PlayFeedbacks();
        }
    }
    
    [SerializeField] private float maxDistanceFromSun = 1000f;
    private void CheckDistanceFromSun()
    {
        if (_sun is null)
            return;
        
        var distance = Vector3.Distance(transform.position, _sun.transform.position);
        if (distance > maxDistanceFromSun)
            Die();
    }
    
    // Getter and setter
    public bool IsPlanetHidden() { return _isHidden; }
}