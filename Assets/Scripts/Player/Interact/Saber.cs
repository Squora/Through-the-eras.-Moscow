using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saber : MonoBehaviour, IInteractable
{
    [SerializeField] private PopupSystem _popupSystem;
    [SerializeField] private string _prompt;
    [SerializeField] private bool _canBeHeldInHand;

    public bool CanBeHeldInHand => _canBeHeldInHand;
    public string InteractionPrompt => _prompt;

    public bool Interact(Interactor interactor)
    {
        var inventory = interactor.GetComponent<Inventory>();
        if (inventory == null) return false;

        if (inventory.HasKey)
        {
            //_popupSystem.AddPopupAlert("Внимание!", "Вы подобрали саблю", 5f);
            Debug.Log("Saber was taken");
            inventory.AddThing(gameObject);
            //gameObject.SetActive(false);
            return true;
        }

        Debug.Log("No key found!");
        return false;
    }
}
