using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : MonoBehaviour, IInteractable
{
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
            GameObject.FindObjectOfType<AIAssistant>().SayInformation
            ("Вы подняли: французское кремневое гладкоствольное пехотное ружье." +
            " \r\nМасса: 4,4 кг.\r\nКалибр: 17.5 мм\r\nВыстрел происходит с " +
            "помощью искр ударного кремневого замка, также именуемого батарейным." +
            "\r\nПрицельная дальность: 150 метров.\r\n\r\nИспользование: на " +
            "перезарядку данного оружия вам потребуется более минуты – поэтому " +
            "я рекомендую использовать его в качестве однозарядного и не перезаряжать" +
            " во время боя. \r\nПотому что в зависимости от числа и вооружения " +
            "противников – за время перезарядки вас могут убить от 2 до 4 раз. \r\nСделать" +
            " повторный выстрел возможно в тех ситуациях, когда вы стреляете в противника" +
            " с максимальной дистанции и противник не имеет огнестрельного оружия.\r\n\r\n" +
            "Прицелиться — нажмите RMB\r\nВыстрелить — нажмите LMB", 10f);
            inventory.AddThing(gameObject);
            gameObject.SetActive(false);
            return true;
        }

        Debug.Log("No key found!");
        return false;
    }
}
