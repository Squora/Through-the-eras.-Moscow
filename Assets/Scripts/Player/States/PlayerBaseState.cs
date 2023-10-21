using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState : MonoBehaviour
{
    public abstract void EnterState(PlayerStateManager stateManager);
    public abstract void UpdateState(PlayerStateManager stateManager);
}
