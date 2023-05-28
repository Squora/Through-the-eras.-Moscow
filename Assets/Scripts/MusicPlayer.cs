using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _playlist;
    [SerializeField] private int _currentMusicIndex = 0;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _currentMusicIndex = 0;
        _audioSource.clip = _playlist[_currentMusicIndex];
        _audioSource.Play();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            if (_currentMusicIndex < _playlist.Length - 1) _currentMusicIndex++;
            else _currentMusicIndex = 0;
            _audioSource.clip = _playlist[_currentMusicIndex];
            _audioSource.Play();
        }
        if (!_audioSource.isPlaying && _audioSource.time == 0) _currentMusicIndex++;
        else if (_currentMusicIndex > _playlist.Length - 1) _currentMusicIndex = 0;
    }
}
