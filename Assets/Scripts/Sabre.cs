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
            ("Вы подняли: палаш\r\nМасса: 1,35 кг без ножен.\r\nНаносит как рубящие, " +
            "так и колющие удары. Также может блокировать удары холодного оружия противников." +
            "\r\nЗа счет своей массы и клинка способен пробивать кирасы и любую защиту " +
            "противников – но я не совсем уверен в ваших физических способностях, поэтому " +
            "гарантировать стабильность такого удара не могу.\r\nУдар – нажмите [ЛКМ]", 10f);
        var inventory = interactor.GetComponent<Inventory>();
        if (inventory == null) return false;

        if (inventory.HasKey)
        {
            Debug.Log("Saber was taken");
            inventory.AddThing(gameObject);
            gameObject.layer = 0;
            return true;
        }

        Debug.Log("No key found!");
        return false;
    }
}
