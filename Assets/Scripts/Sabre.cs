using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sabre : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;
    [SerializeField] private bool _canBeHeldInHand;

    public bool CanBeHeldInHand => _canBeHeldInHand;
    public string InteractionPrompt => _prompt;

    public bool Interact(Interactor interactor)
    {
        GameObject.FindObjectOfType<AIAssistant>().SayInformation
            ("¬ы подн€ли: палаш\r\nћасса: 1,35 кг без ножен.\r\nЌаносит как руб€щие, " +
            "так и колющие удары. “акже может блокировать удары холодного оружи€ противников." +
            "\r\n«а счет своей массы и клинка способен пробивать кирасы и любую защиту " +
            "противников Ц но € не совсем уверен в ваших физических способност€х, поэтому " +
            "гарантировать стабильность такого удара не могу.\r\n”дар Ц нажмите Е\r\n—ильный " +
            "удар Ц нажмите Е\r\nЅлок Ц нажмите Е", 10f);
        var inventory = interactor.GetComponent<Inventory>();
        if (inventory == null) return false;

        if (inventory.HasKey)
        {
            //_popupSystem.AddPopupAlert("¬нимание!", "¬ы подобрали саблю", 5f);
            Debug.Log("Saber was taken");
            inventory.AddThing(gameObject);
            gameObject.layer = 0;
            return true;
        }

        Debug.Log("No key found!");
        return false;
    }
}
