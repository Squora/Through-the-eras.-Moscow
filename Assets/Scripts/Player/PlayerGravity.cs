using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

[RequireComponent(typeof(CharacterController))]
public class PlayerGravity : MonoBehaviour
{
    public bool IsEnabled = true;
    
    private CharacterController _controller;

    [SerializeField] private float _groundYOffset;
    [SerializeField] LayerMask _groundMask;
    private Vector3 _spherePosition;

    [SerializeField] private float _gravity = -9.81f;
    private Vector3 _velocity;

    [SerializeField] private float _groundCheckRadius = 0.3f;
    [SerializeField] private Color _groundCheckSphereColor = Color.red;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (IsEnabled)
        {
            Gravity();
        }
    }

    private bool IsGrounded()
    {
        _spherePosition = new Vector3(transform.position.x, transform.position.y - _groundYOffset, transform.position.z);
        if (Physics.CheckSphere(_spherePosition, _groundCheckRadius, _groundMask)) return false;
        return true;
    }

    private void Gravity()
    {
        if (!IsGrounded()) _velocity.y += _gravity * Time.deltaTime;
        else if (_velocity.y < 0) _velocity.y = -2;

        _controller.Move(_velocity * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _groundCheckSphereColor;
        Gizmos.DrawWireSphere(_spherePosition, _groundCheckRadius);
    }
}
