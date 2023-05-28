using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private bool _gameIsPaused;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !_gameIsPaused) PauseGame();
        else if (Input.GetKeyDown(KeyCode.Escape) && _gameIsPaused) ResumeGame();
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
        _gameIsPaused = true;
    }
    private void ResumeGame()
    {
        Time.timeScale = 1;
        _gameIsPaused = false;
    }
}
