using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameManager _gameManager;
    private PlacingBallController _placingBallController;
    private CueController _cueController;

    private Controls _controls;

    public void Initialize()
    {
        _gameManager = GameManager.instance;
        _placingBallController = _gameManager.GetPlacingBallController();
        _cueController = _gameManager.GetCueController();
        
        _controls = new Controls();
        _controls.Player.LeftClick.performed += _ => PressLeftClick();
        _controls.Player.LeftClick.canceled += _ => ReleaseLeftClick();
        _controls.Enable();
    }
    
    private void PressLeftClick()
    {
        switch (_gameManager.GetGameState())
        {
            case GameManager.GameState.PlacingBall:
                GameManager.instance.SetState(GameManager.GameState.Charging);
                break;
            case GameManager.GameState.Charging:
                break;
            case GameManager.GameState.Shooting:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    private void ReleaseLeftClick()
    {
        switch (_gameManager.GetGameState())
        {
            case GameManager.GameState.PlacingBall:
                break;
            case GameManager.GameState.Charging:
                _cueController.ApplyForce();
                break;
            case GameManager.GameState.Shooting:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
