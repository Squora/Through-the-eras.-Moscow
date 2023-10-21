using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent (typeof(Animator))]
public class PlayerStateManager : MonoBehaviour
{
    [HideInInspector] public PlayerIdleState IdleState;
    [HideInInspector] public PlayerWalkState WalkState;
    [HideInInspector] public PlayerRunState RunState;
    [HideInInspector] public PlayerDodgeState DodgeState;

    [SerializeField] private PlayerBaseState _currentState;

    public float Speed = 0f;

    public CharacterController Controller;
    public PlayerInput PlayerInput;
    public Animator AnimatorController;

    //[SerializeField] private float rotationSpeed = 0.08f;
    //private Transform cameraTransform;

    private void Start()
    {
        IdleState = GetComponent<PlayerIdleState>();
        WalkState = GetComponent<PlayerWalkState>();
        RunState = GetComponent<PlayerRunState>();
        DodgeState = GetComponent<PlayerDodgeState>();

        //cameraTransform = Camera.main.transform;

        Controller = GetComponent<CharacterController>();
        PlayerInput = GetComponent<PlayerInput>();
        AnimatorController = GetComponent<Animator>();
        SwitchState(IdleState);
    }

    private void Update()
    {

        //Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
        //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        _currentState.UpdateState(this);
    }

    public void SwitchState(PlayerBaseState state)
    {
        _currentState = state;
        _currentState.EnterState(this);
    }
}
