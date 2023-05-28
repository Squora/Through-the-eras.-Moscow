using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeCombat : MonoBehaviour
{
    [Header("Attack parameters")]
    [SerializeField] private int _damage = 10;
    [SerializeField] private float _hitCooldown = 2f;
    [SerializeField] private bool _canAttack = true;
    [SerializeField] private float _attackTimer;
    [SerializeField] private float _hitDelay = 0;
    [SerializeField] private float _aimingTime = 0;

    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (GetComponent<Enemy>().PlayerIsOnAttackDistantion)
        {
            Attack();
        }
        ResetAttackCooldown();
    }

    public void Attack()
    {
        GetComponent<Enemy>().Agent.isStopped = true;
        _animator.SetTrigger("Aim");
        Invoke("Shoot", _aimingTime);
    }

    private void Shoot()
    {
        if (_canAttack)
        {
            _animator.SetTrigger("Attack");
            _canAttack = false;
            Invoke("HitPlayer", _hitDelay);
        }
    }

    private void HitPlayer()
    {
        GetComponent<Enemy>().Player.GetComponent<PlayerHealth>().TakeDamage(_damage);
    }

    private void ResetAttackCooldown()
    {
        if (!_canAttack)
        {
            if (_attackTimer < _hitCooldown)
            {
                GetComponent<Enemy>().Agent.isStopped = true;
                _attackTimer += Time.deltaTime;
            }
            else
            {
                GetComponent<Enemy>().Agent.isStopped = false;
                _attackTimer = 0;
                _canAttack = true;
            }
        }
    }
}
