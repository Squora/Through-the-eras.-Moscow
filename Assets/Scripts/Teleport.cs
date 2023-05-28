using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{
    [SerializeField] private AudioSource _transitionAudio;
    [SerializeField] private GameObject _panelSmoothlyTransition;
    [SerializeField] private float _delay;
    public string SceneName;

    private void Awake()
    {
        _panelSmoothlyTransition.GetComponent<Animator>().SetBool("IsSceneLoaded", false);
    }

    private void Start()
    {
        _transitionAudio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            _transitionAudio.Play();
            _panelSmoothlyTransition.GetComponent<Animator>().Play("SmoothlyTransition");
            GameObject.FindObjectOfType<AIAssistant>().SayInformation(
                "ИИ: Так и было задумано..?", 5);
            Invoke("LoadScene", _delay);
        }
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(SceneName, LoadSceneMode.Single);
    }
}
