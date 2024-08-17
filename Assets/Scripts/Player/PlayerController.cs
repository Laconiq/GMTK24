using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private PlacingBallController placingBallController;
    [SerializeField] private CueController cueController;

    private Controls _controls;

    public void Initialize()
    {
        _controls = new Controls();
        _controls.Player.LeftClick.performed += _ => PressLeftClick();
        _controls.Player.LeftClick.canceled += _ => ReleaseLeftClick();
        _controls.Enable();
    }
    
    private void PressLeftClick()
    {
        switch (gameManager.GetGameState())
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
        switch (gameManager.GetGameState())
        {
            case GameManager.GameState.PlacingBall:
                break;
            case GameManager.GameState.Charging:
                cueController.ApplyForce();
                break;
            case GameManager.GameState.Shooting:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
