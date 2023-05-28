using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    [Header("Interact parameters")]
    [SerializeField] private Transform _interactionPoint;
    [SerializeField] private float _interactionPointRaduis = 0.5f;
    [SerializeField] private LayerMask _interactionMask;
    [Header("Gizmos parameters")]
    [SerializeField] private Color _interactionColor = Color.red;

    private readonly Collider[] _colliders = new Collider[3];
    [SerializeField] private int _numFound;
    [SerializeField] private GameObject _uiHelper;

    private StarterAssetsInputs _input;

    private void Start()
    {
        _input = GetComponent<StarterAssetsInputs>();
    }

    private void Update()
    {
        _numFound = Physics.OverlapSphereNonAlloc(_interactionPoint.position, _interactionPointRaduis,
            _colliders, _interactionMask);

        if (_numFound > 0)
        {
            _uiHelper.SetActive(true);
            var interactable = _colliders[0].GetComponent<IInteractable>();

            if (interactable != null && _input.interact)
            {
                interactable.Interact(this);
            }
        }
        else _uiHelper.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _interactionColor;
        Gizmos.DrawWireSphere(_interactionPoint.position, _interactionPointRaduis);
    }
}
