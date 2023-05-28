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
        if (_audioSource.isPlaying == false)
        {
            _currentMusicIndex++;
            if (_currentMusicIndex > _playlist.Length) _currentMusicIndex = 0;
            _audioSource.clip = _playlist[_currentMusicIndex];
            _audioSource.Play();
        }
    }
}
