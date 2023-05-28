using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class MeleeCombat : MonoBehaviour
{
    [Header("Attack parameters")]
    [SerializeField] private int _damage = 10;
    [SerializeField] private float _hitCooldown = 2f;
    [SerializeField] private bool _canAttack = true;
    [SerializeField] private float _attackTimer;
    [SerializeField] private int _attackCount = 1;
    [SerializeField] private float _hitDelay = 0;

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
        if (_canAttack)
        {
            GetComponent<Enemy>().Agent.isStopped = true;
            int currentAttack = Random.Range(0, _attackCount);
            switch (currentAttack)
            {
                case 0:
                    _animator.SetTrigger("Attack");
                    break;
                case 1:
                    _animator.SetTrigger("Attack2");
                    _damage = 50;
                    break;
            }
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
