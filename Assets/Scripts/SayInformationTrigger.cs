using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SayInformationTrigger : MonoBehaviour
{
    public string Text;
    public float Delay;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInChildren<AIAssistant>())
        {
            other.GetComponentInChildren<AIAssistant>().SayInformation(Text, Delay);
        }
    }
}
