using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    private CueController _cueController;
    private PlacingBallController _placingBallController;
    private PlayerController _playerController;
    private CameraController _cameraController;
    private UIManager _uiManager;
    private Planet _currentPlanet;
    private GameState _gameState;

    public enum GameState
    {
        PlacingBall,
        Charging,
        Shooting
    }
    
    private void Awake()
    {
        if (instance is null)
            Initialize();
        else
            Destroy(gameObject);
    }

    private void Initialize()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        
        _playerController = FindObjectOfType<PlayerController>();
        if (_playerController is null)
            Debug.LogError("PlayerController not found in scene");
        
        _cameraController = FindObjectOfType<CameraController>();
        if (_cameraController is null)
            Debug.LogError("CameraController not found in scene");
        
        _cueController = FindObjectOfType<CueController>();
        if (_cueController is null)
            Debug.LogError("CueController not found in scene");
        
        _placingBallController = FindObjectOfType<PlacingBallController>();
        if (_placingBallController is null)
            Debug.LogError("PlacingBallController not found in scene");
        
        _uiManager = FindObjectOfType<UIManager>();
        if (_uiManager is null)
            Debug.LogError("UIManager not found in scene");
        
        _uiManager?.Initialize();
        _playerController?.Initialize();
        _cameraController?.Initialize();
    }

    private void Start()
    {
        SetState(GameState.PlacingBall);
    }

    public void SetState(GameState state)
    {
        _gameState = state;
        
        switch (_gameState)
        {
            case GameState.PlacingBall:
                _cueController.DisableControls();
                _placingBallController.EnableControls();
                break;
            case GameState.Charging:
                _cueController.EnableControls();
                _placingBallController.DisableControls();
                break;
            case GameState.Shooting:
                _cueController.DisableControls();
                _placingBallController.DisableControls();
                SetCurrentPlanet(null);
                Invoke(nameof(SetStatePlacingBall), 3f);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    // Getters and Setters
    private void SetStatePlacingBall() { SetState(GameState.PlacingBall); }
    public void SetCurrentPlanet(Planet planet) { _currentPlanet = planet; }
    public Planet GetCurrentPlanet() { return _currentPlanet; }
    public PlacingBallController GetPlacingBallController() { return _placingBallController; }
    public UIManager GetUIManager() { return _uiManager; }
    public CueController GetCueController() { return _cueController; }
    public CameraController GetCameraController() { return _cameraController; }
    public GameState GetGameState() { return _gameState; }
    public PlayerController GetPlayerController() { return _playerController; }
}
