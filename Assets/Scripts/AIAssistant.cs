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
    [Header("Blinding ability parameters")]
    [SerializeField] private bool _blindingAbilityIsReady = true;
    [SerializeField] private int _blindingAbilityDuration = 5;
    [SerializeField] private int _blindingAbilityCooldown = 20;
    [SerializeField] private int _blindingAbilityRadius = 5;
    [SerializeField] private LayerMask _blindingAbilityMask;
    [Header("Gizmos parameters")]
    [SerializeField] private Color _blindingAbilityColor_ =  Color.yellow;

    private AudioSource _audioSource;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
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
        _audioSource.Play();
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
            _shieldAbilityIsReady = false;
            GetComponentInParent<PlayerHealth>().CanBeDamaged = false;
            yield return new WaitForSeconds(_shieldAbilityDuration);
            GetComponentInParent<PlayerHealth>().CanBeDamaged = true;
            yield return new WaitForSeconds(_shieldAbilityCooldown);
            _shieldAbilityIsReady = true;
        }
    }

    public IEnumerator BlindingAbility()
    {
        if (_blindingAbilityIsReady)
        {
            _blindingAbilityIsReady = false;
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
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _blindingAbilityColor_;
        Gizmos.DrawWireSphere(gameObject.transform.position, _blindingAbilityRadius);
    }
}
