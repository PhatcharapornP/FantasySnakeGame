using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerInput : MonoBehaviour,GameInputAsset.IPlayerActions
{
    private GameInputAsset inputAction;
    [SerializeField] private Vector2Int moveValue;

    private void Awake()
    {
        inputAction = new GameInputAsset();
        inputAction.Player.SetCallbacks(this);
        inputAction.Player.Enable();
    }
    
    public void OnMove(InputAction.CallbackContext context)
    {
        moveValue.x = (int)context.ReadValue<Vector2>().x;
        moveValue.y = (int)context.ReadValue<Vector2>().y;
        Player.Instance.MovePartyLeader(moveValue);
    }

    public void OnSwitchHero(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            Debug.Log($"gimme switch hero".InColor(Color.magenta));   
        }
    }

    public void OnRotateHero(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
         Debug.Log($"yoooooooo rotate pls".InColor(Color.cyan));
        }
    }

    private void Update()
    {
        
    }
}
