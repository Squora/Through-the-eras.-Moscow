
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Abilities : MonoBehaviour
{
    [SerializeField] private Image _shieldAbility;
    [SerializeField] private Image _blindAbility;
    [SerializeField] private Image _pistolAbility;

    public void ShowShieldAbility(bool show)
    {
        _shieldAbility.enabled = show;
    }

    public void ShowBlindAbility(bool show)
    {
        _blindAbility.enabled = show;
    }
    public void ShowPistolAbility(bool show)
    {
        _pistolAbility.enabled = show;
    }
}
