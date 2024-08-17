using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    private GameState _gameState;
    private Planet _currentPlanet;
    
    public void SetCurrentPlanet(Planet planet)
    {
        _currentPlanet = planet;
    }
    
    public Planet GetCurrentPlanet()
    {
        return _currentPlanet;
    }

    public enum GameState
    {
        PlacingBall,
        Charging,
        Shooting
    }
    
    [SerializeField] private CueController cueController;
    [SerializeField] private PlacingBallController placingBallController;
    
    private void Awake()
    {
        if (instance is null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Initialize()
    {
        cueController.Initialize();
        placingBallController.Initialize();
        SetState(GameState.PlacingBall);
    }
    
    public void SetState(GameState state)
    {
        _gameState = state;
        
        switch (_gameState)
        {
            case GameState.PlacingBall:
                cueController.DisableControls();
                placingBallController.EnableControls();
                Debug.Log("State is PlacingBall");
                break;
            case GameState.Charging:
                cueController.EnableControls();
                placingBallController.DisableControls();
                Debug.Log("State is Charging");
                break;
            case GameState.Shooting:
                cueController.DisableControls();
                placingBallController.DisableControls();
                SetCurrentPlanet(null);
                Debug.Log("State is Shooting");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
