using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{
    public bool HasKey = false;

    [SerializeField] private Transform _handTransform;
    public List<GameObject> InventoryList = new List<GameObject>();
    [SerializeField] private int _inventorySize = 5;
    [SerializeField] private GameObject _inHandObject;

    private void Start()
    { 

    }

    private void Update()
    {
        //if (Keyboard.current.qKey.wasPressedThisFrame) HasKey = !HasKey;
        //if (Input.GetKeyDown(KeyCode.Q) && _inHandObject != null)
        //{
        //    InventoryList.Remove(_inHandObject);
        //    _inHandObject.transform.SetParent(null);
        //    _inHandObject = null;
        //}
    }

    public void AddThing(GameObject thing)
    {
        if (InventoryList.Count < _inventorySize)
        {
            InventoryList.Add(thing);
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
