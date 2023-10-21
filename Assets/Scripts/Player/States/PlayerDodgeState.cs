using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.Windows;

public class PlayerDodgeState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager stateManager)
    {
        stateManager.AnimatorController.SetTrigger("DodgeForward");
    }
    
    public override void UpdateState(PlayerStateManager stateManager)
    {
        if (true)
        {
            stateManager.SwitchState(stateManager.IdleState);
        }
    }
}
