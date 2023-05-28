using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rapier : MonoBehaviour
{
    public string PlayerTag = "Player";
    public int Damage = 10;

    //private void Start()
    //{
    //    GetComponent<BoxCollider>().enabled = false;
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.tag == PlayerTag)
    //    {
    //        var healthComponent = other.GetComponent<Health>();
    //        healthComponent.TakeDamage(Damage);
    //        GetComponent<BoxCollider>().enabled = false;
    //    }
    //}
}
