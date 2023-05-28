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
            ("�� �������: ����������� ��������� ��������������� �������� �����." +
            " \r\n�����: 4,4 ��.\r\n������: 17.5 ��\r\n������� ���������� � " +
            "������� ���� �������� ���������� �����, ����� ���������� ����������." +
            "\r\n���������� ���������: 150 ������.\r\n\r\n�������������: �� " +
            "����������� ������� ������ ��� ����������� ����� ������ � ������� " +
            "� ���������� ������������ ��� � �������� ������������� � �� ������������" +
            " �� ����� ���. \r\n������ ��� � ����������� �� ����� � ���������� " +
            "����������� � �� ����� ����������� ��� ����� ����� �� 2 �� 4 ���. \r\n�������" +
            " ��������� ������� �������� � ��� ���������, ����� �� ��������� � ����������" +
            " � ������������ ��������� � ��������� �� ����� �������������� ������.\r\n\r\n" +
            "����������� � ������� RMB\r\n���������� � ������� LMB", 10f);
            inventory.AddThing(gameObject);
            gameObject.SetActive(false);
            return true;
        }

        Debug.Log("No key found!");
        return false;
    }
}
