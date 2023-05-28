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
            ("�� �������: ����������� ��������� ��������������� ��������. \r\n�����: 1,3 ��." +
            "\r\n������: 17.1 ��\r\n������� ���������� � ������� ���� �������� ���������� " +
            "�����, ����� ���������� ����������.\r\n���������� ���������: ������ 50 ������ " +
            "�������� ������������.\r\n�������������: �� ����������� ������� ������ ��� " +
            "����������� ����� ������ � ������� � ���������� ������������ ��� � �������� " +
            "������������� � �� �������������� � ���� ���. � ����������� �� ����� � " +
            "���������� ����������� � �� ����� ����������� ��� ����� ����� �� 1 �� 3 ���." +
            "\r\n������� � ������� �\r\n������������ � ������� �", 15f);
        var inventory = interactor.GetComponent<Inventory>();
        if (inventory == null) return false;

        if (inventory.HasKey)
        {
            //_popupSystem.AddPopupAlert("��������!", "�� ��������� �����", 5f);
            Debug.Log("Saber was taken");
            inventory.AddThing(gameObject);
            gameObject.SetActive(false);
            return true;
        }

        Debug.Log("No key found!");
        return false;
    }
}
