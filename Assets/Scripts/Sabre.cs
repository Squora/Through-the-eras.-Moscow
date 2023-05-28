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
            ("�� �������: �����\r\n�����: 1,35 �� ��� �����.\r\n������� ��� �������, " +
            "��� � ������� �����. ����� ����� ����������� ����� ��������� ������ �����������." +
            "\r\n�� ���� ����� ����� � ������ �������� ��������� ������ � ����� ������ " +
            "����������� � �� � �� ������ ������ � ����� ���������� ������������, ������� " +
            "������������� ������������ ������ ����� �� ����.\r\n���� � ������� [���]", 10f);
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
