using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    [SerializeField] private CueController cueController;
    [SerializeField] private PlacingBallController placingBallController;
    [SerializeField] private PlayerController playerController;
    
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
    
    public PlacingBallController GetPlacingBallController()
    {
        return placingBallController;
    }

    public enum GameState
    {
        PlacingBall,
        Charging,
        Shooting
    }
    
    public GameState GetGameState()
    {
        return _gameState;
    }
    
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
        playerController.Initialize();
        SetState(GameState.PlacingBall);
    }
    
    public void SetState(GameState state)
    {
        _gameState = state;
        
        Debug.Log("State changed to " + _gameState);
        
        switch (_gameState)
        {
            case GameState.PlacingBall:
                cueController.DisableControls();
                placingBallController.EnableControls();
                break;
            case GameState.Charging:
                cueController.EnableControls();
                placingBallController.DisableControls();
                break;
            case GameState.Shooting:
                cueController.DisableControls();
                placingBallController.DisableControls();
                SetCurrentPlanet(null);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
