using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
public class Health : MonoBehaviour
{
    [Header("Health parameters")]
    public bool CanBeDamaged = true;
    public bool IsAlive = true;
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _currentHealth;
    [Header("Health regeneration parameters")]
    [SerializeField] private bool _canRegenerateHealth;
    [SerializeField] private float _delayBeforeRegenerateHealth;
    [SerializeField] private float _healthPerSecond;
    [Header("Other")]
    [SerializeField] private string _animationHitName;
    private Animator _animator;

    private float _timer;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _currentHealth = _maxHealth;
        IsAlive = true;
    }

    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        if (CanBeDamaged)
        {
            if (_currentHealth <= 0)
            {
                Die();
                IsAlive = false;
            }
            else
            {
                _animator.SetTrigger("Hurt");
                _currentHealth -= damage;
            }
        }
    }

    private void Regenerate()
    {

    }

    private void Die()
    {
        _animator.SetTrigger("Death"); 
        gameObject.GetComponent<Enemy>().enabled = false;
        gameObject.GetComponent<MeleeCombat>().enabled = false;
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        gameObject.GetComponent<BoxCollider>().enabled = false;
        gameObject.GetComponent<NavMeshAgent>().isStopped = true;
        enabled = false;
    }
}
