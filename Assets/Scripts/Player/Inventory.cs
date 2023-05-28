using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{
    public bool HasKey = false;

    [SerializeField] private Transform _handTransform;
    [SerializeField] private List<GameObject> _inventory = new List<GameObject>();
    [SerializeField] private int _inventorySize = 5;
    [SerializeField] private GameObject _inHandObject;

    private void Start()
    { 

    }

    private void Update()
    {
        //if (Keyboard.current.qKey.wasPressedThisFrame) HasKey = !HasKey;
        if (Input.GetKeyDown(KeyCode.Q) && _inHandObject != null)
        {
            _inventory.Remove(_inHandObject);
            _inHandObject.transform.SetParent(null);
            _inHandObject = null;
        }
    }

    public void AddThing(GameObject thing)
    {
        if (_inventory.Count < _inventorySize)
        {
            _inventory.Add(thing);
            if (thing.GetComponent<IInteractable>().CanBeHeldInHand && _inHandObject == null)
            {
                thing.transform.position = Vector3.zero;
                thing.transform.rotation = Quaternion.Euler(0, 0, 90);
                thing.transform.SetParent(_handTransform, false);
                _inHandObject = thing;
            }
        }
        else
        {
            Debug.Log("Недостаточно места в инвентаре");
        }
    }
}
