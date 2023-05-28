using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator))]
public class PlayerHealth : MonoBehaviour
{
    [Header("Health parameters")]
    public bool CanBeDamaged = true;
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _currentHealth;
    [Header("Health regeneration parameters")]
    [SerializeField] private bool _canRegenerateHealth;
    [SerializeField] private float _delayBeforeRegenerateHealth;
    [SerializeField] private float _healthPerSecond;
    [Header("Other")]
    [SerializeField] private string _animationHitName;
    [SerializeField] private GameObject _deathPanel;
    [SerializeField] private GameObject _healthBar;
    private Animator _animator;

    private float _timer;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _currentHealth = _maxHealth;
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
            }
            else
            {
                _animator.SetTrigger("Hurt");
                _currentHealth -= damage;
                _healthBar.GetComponent<HealthBar>().SetHealth(_currentHealth);
            }
        }
    }

    private void Regenerate()
    {

    }

    private void Die()
    {
        _animator.SetTrigger("Death");
        CanBeDamaged = false;
        _deathPanel.SetActive(true);
        _deathPanel.GetComponent<Animator>().SetTrigger("Death");
        gameObject.GetComponent<ThirdPersonController>().enabled = false;
        gameObject.GetComponent<CharacterController>().enabled = false;
        gameObject.GetComponent<StarterAssetsInputs>().enabled = false;
        enabled = false;
        Invoke("LoadScene", 5f);
    }

    private void LoadScene()
    {
        SceneManager.LoadScene("Moscow1812Scene", LoadSceneMode.Single);
    }
}
