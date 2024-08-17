using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private GameManager _gameManager;
    private CueController _cueController;
    private UIManager _uiManager;
    private Coroutine _checkUIElementUnderMouseCoroutine;

    private Controls _controls;

    public void Initialize()
    {
        _gameManager = GameManager.instance;
        _cueController = _gameManager.GetCueController();
        _uiManager = _gameManager.GetUIManager();

        _controls = new Controls();
        _controls.Player.LeftClick.performed += _ => PressLeftClick();
        _controls.Player.LeftClick.canceled += _ => ReleaseLeftClick();
        _controls.Player.RightClick.performed += _ => PressRightClick();
        _controls.Player.RightClick.canceled += _ => ReleaseRightClick();
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

    private void PressRightClick()
    {
        switch (_gameManager.GetGameState())
        {
            case GameManager.GameState.PlacingBall:
                _checkUIElementUnderMouseCoroutine = StartCoroutine(CheckUIElementUnderMouseCoroutine());
                _uiManager.OpenRadialMenu(true);
                break;
            case GameManager.GameState.Charging:
                break;
            case GameManager.GameState.Shooting:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void ReleaseRightClick()
    {
        switch (_gameManager.GetGameState())
        {
            case GameManager.GameState.PlacingBall:
                StopCoroutine(_checkUIElementUnderMouseCoroutine);
                _uiManager.OpenRadialMenu(false);
                break;
            case GameManager.GameState.Charging:
                break;
            case GameManager.GameState.Shooting:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    private Button _lastButtonClicked;
    private IEnumerator CheckUIElementUnderMouseCoroutine()
    {
        while (Mouse.current.rightButton.isPressed)
        {
            var pointerData = new PointerEventData(EventSystem.current)
            {
                position = Mouse.current.position.ReadValue()
            };

            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            foreach (var result in results)
                if (result.gameObject.TryGetComponent(out Button button) && button != _lastButtonClicked)
                {
                    _lastButtonClicked = button;
                    button.onClick.Invoke();
                    break;
                }
            yield return null;
        }
    }
}