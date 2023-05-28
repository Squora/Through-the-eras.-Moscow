using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour, IInteractable
{
    [Header("Interact parameters")]
    [SerializeField] private string _prompt;
    [SerializeField] private bool _canBeHeldInHand;
    public bool CanBeHeldInHand => _canBeHeldInHand;
    public string InteractionPrompt => _prompt;

    public bool Interact(Interactor interactor)
    {
        GameObject.FindObjectOfType<AIAssistant>().SayInformation
            ("Вы подняли: французский кремневый гладкоствольный пистолет. \r\nМасса: 1,3 кг." +
            "\r\nКалибр: 17.1 мм\r\nВыстрел происходит с помощью искр ударного кремневого " +
            "замка, также именуемого батарейным.\r\nПрицельная дальность: дальше 50 метров " +
            "стрелять бессмысленно.\r\nИспользование: на перезарядку данного оружия вам " +
            "потребуется около минуты – поэтому я рекомендую использовать его в качестве " +
            "однозарядного и не перезаряжаться в ходе боя. В зависимости от числа и " +
            "вооружения противников – за время перезарядки вас могут убить от 1 до 3 раз." +
            "\r\nВыстрел – нажмите …\r\nПерезарядить – нажмите …", 15f);
        var inventory = interactor.GetComponent<Inventory>();
        if (inventory == null) return false;

        if (inventory.HasKey)
        {
            //_popupSystem.AddPopupAlert("Внимание!", "Вы подобрали саблю", 5f);
            Debug.Log("Saber was taken");
            inventory.AddThing(gameObject);
            gameObject.SetActive(false);
            return true;
        }

        Debug.Log("No key found!");
        return false;
    }
}
