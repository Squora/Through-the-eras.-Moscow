using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.Windows;

public class PlayerWalkState : PlayerBaseState
{
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private Vector3 _direction;


    public InputAction _moveAction;
    public bool _runAction;
    public bool _dodgeAction;
    //
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;
    private float _verticalVelocity;
    public float RotationSmoothTime = 0.12f;

    public override void EnterState(PlayerStateManager stateManager)
    {
        _moveAction = stateManager.PlayerInput.actions["Move"];
        stateManager.AnimatorController.SetFloat("Speed", 1f);
        stateManager.Speed = _moveSpeed;
    }
    
    public override void UpdateState(PlayerStateManager stateManager)
    {
        _runAction = stateManager.PlayerInput.actions["Run"].IsPressed();
        _dodgeAction = stateManager.PlayerInput.actions["Dodge"].IsPressed();

        Vector2 input = _moveAction.ReadValue<Vector2>();
        _direction = transform.forward * input.y + transform.right * input.x;

        if (_direction.magnitude < 0.1f)
        {
            stateManager.SwitchState(stateManager.IdleState);
        }
        if (_runAction)
        {
            stateManager.SwitchState(stateManager.RunState);
        }
        if (_dodgeAction)
        {
            stateManager.SwitchState(stateManager.DodgeState);
        }

        Vector3 inputDirection = new Vector3(input.x, 0.0f, input.y).normalized;
        _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
          Camera.main.transform.eulerAngles.y;
        float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
            RotationSmoothTime);

        transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);

        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
        stateManager.Controller.Move(targetDirection.normalized * (_moveSpeed * Time.deltaTime) +
            new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
    }
}
