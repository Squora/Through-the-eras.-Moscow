using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AIAssistant : MonoBehaviour
{
    [Header("Popup parameters")]
    [SerializeField] private GameObject _panel;
    [SerializeField] private TextMeshProUGUI _text;
    [Header("Shield ability parameters")]
    [SerializeField] private bool _shieldAbilityIsReady = true;
    [SerializeField] private int _shieldAbilityDuration = 5;
    [SerializeField] private int _shieldAbilityCooldown = 20;
    [SerializeField] private AudioClip _shieldAbilityAudioClip;
    [Header("Blinding ability parameters")]
    [SerializeField] private bool _blindingAbilityIsReady = true;
    [SerializeField] private int _blindingAbilityDuration = 5;
    [SerializeField] private int _blindingAbilityCooldown = 20;
    [SerializeField] private int _blindingAbilityRadius = 5;
    [SerializeField] private AudioClip _blindingAbilityAudioClip;
    [SerializeField] private LayerMask _blindingAbilityMask;
    [Header("Other")]
    [SerializeField] private AudioClip _popUpInformationAudioClip;
    [SerializeField] private Abilities _abilities;
    private AudioSource _audioSource;
    [Header("Gizmos parameters")]
    [SerializeField] private Color _blindingAbilityColor_ =  Color.yellow;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine(ShieldAbility());
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            StartCoroutine(BlindingAbility());
        }
    }

    public void SayInformation(string text, float delay)
    {
        _audioSource.clip = _popUpInformationAudioClip;
        _audioSource.Play();

        Debug.Log(text.Length);
        if (text.Length < 350) _text.fontSize = 20;
        else if (text.Length < 600) _text.fontSize = 14;
        else _text.fontSize = 12;

        _text.text = text;
        _panel.SetActive(true);
        StartCoroutine(HideInformation(delay));
    }

    private IEnumerator HideInformation(float delay)
    {
        yield return new WaitForSeconds(delay);
        _panel.SetActive(false);
    }

    public IEnumerator ShieldAbility()
    {
        if (_shieldAbilityIsReady)
        {
            _audioSource.clip = _shieldAbilityAudioClip;
            _audioSource.Play();
            _shieldAbilityIsReady = false;
            _abilities.ShowShieldAbility(_shieldAbilityIsReady);
            GetComponentInParent<PlayerHealth>().CanBeDamaged = false;
            yield return new WaitForSeconds(_shieldAbilityDuration);
            GetComponentInParent<PlayerHealth>().CanBeDamaged = true;
            yield return new WaitForSeconds(_shieldAbilityCooldown);
            _shieldAbilityIsReady = true;
            _abilities.ShowShieldAbility(_shieldAbilityIsReady);
        }
    }

    public IEnumerator BlindingAbility()
    {
        if (_blindingAbilityIsReady)
        {
            _audioSource.clip = _blindingAbilityAudioClip;
            _audioSource.Play();
            _blindingAbilityIsReady = false;
            _abilities.ShowBlindAbility(_blindingAbilityIsReady);
            Collider[] enemies = Physics.OverlapSphere(gameObject.transform.position, 
                _blindingAbilityRadius, _blindingAbilityMask);
            foreach (var enemy in enemies)
            {
                var enemyMovement = enemy.GetComponent<Enemy>();
                if (enemyMovement != null)
                {
                    enemyMovement._movementType = Enemy.MovementType.None;
                }
            }
            yield return new WaitForSeconds(_blindingAbilityDuration);
            foreach (var enemy in enemies)
            {
                var enemyMovement = enemy.GetComponent<Enemy>();
                if (enemyMovement != null)
                {
                    enemyMovement._movementType = enemyMovement._previousMovementType;
                }
            }
            yield return new WaitForSeconds(_blindingAbilityCooldown);
            _blindingAbilityIsReady = true;
            _abilities.ShowBlindAbility(_blindingAbilityIsReady);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _blindingAbilityColor_;
        Gizmos.DrawWireSphere(gameObject.transform.position, _blindingAbilityRadius);
    }
}
