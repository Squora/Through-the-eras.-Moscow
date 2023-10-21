using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager stateManager)
    {
        stateManager.AnimatorController.SetFloat("Speed", 0f);
        stateManager.Speed = 0f;
    }

    public override void UpdateState(PlayerStateManager stateManager)
    {
        if (stateManager.PlayerInput.actions["Move"].triggered)
        {
            stateManager.SwitchState(stateManager.WalkState);
        }
    }
}
