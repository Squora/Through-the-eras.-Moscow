using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class PlayerRunState : PlayerBaseState
{
    [SerializeField] private float _runSpeed = 6f;
    [SerializeField] private Vector3 _direction;

    [SerializeField] private bool _runAction;

    public InputAction _moveAction;

    private float _targetRotation = 0.0f;
    private float _rotationVelocity;
    private float _verticalVelocity;
    public float RotationSmoothTime = 0.12f;

    public override void EnterState(PlayerStateManager stateManager)
    {
        _moveAction = stateManager.PlayerInput.actions["Move"];
        stateManager.AnimatorController.SetFloat("Speed", 2f);
        stateManager.Speed = _runSpeed;
    }

    public override void UpdateState(PlayerStateManager stateManager)
    {
        _runAction = stateManager.PlayerInput.actions["Run"].IsPressed();

        Vector2 input = _moveAction.ReadValue<Vector2>();
        _direction = transform.forward * input.y + transform.right * input.x;
        //stateManager.Controller.Move(_direction * _runSpeed * Time.deltaTime);

        if (_direction.magnitude < 0.1f)
        {
            stateManager.SwitchState(stateManager.IdleState);
        }
        else if (!_runAction)
        {
            stateManager.SwitchState(stateManager.WalkState);
        }


        Vector3 inputDirection = new Vector3(input.x, 0.0f, input.y).normalized;
        _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
          Camera.main.transform.eulerAngles.y;
        float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
            RotationSmoothTime);

        transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);

        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
        stateManager.Controller.Move(targetDirection.normalized * (_runSpeed * Time.deltaTime) +
            new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
    }
}
