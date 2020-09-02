using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Trigger : MonoBehaviour
{
    [SerializeField] private bool _hasOnlyAudio;
    [SerializeField] private bool _hasCutScene;

    [SerializeField] private bool _freezPlayer;

    private AudioSource _audioSource;
    private PlayableDirector _director;

    private FirstPersonAIO _player;

    void Start()
    {
        if (_hasOnlyAudio)
        {
            _audioSource = this.GetComponent<AudioSource>();
        }
        else if (_hasCutScene)
        {
            _director = this.GetComponent<PlayableDirector>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_hasOnlyAudio)
            {
                _audioSource.Play();
                StartCoroutine(RemoveScript(_audioSource.clip.length));
            }
            else if (_hasCutScene)
            {
                _director.Play();
                StartCoroutine(RemoveScript((float)_director.duration));
            }

            if (_freezPlayer)
            {
                _player = other.GetComponent<FirstPersonAIO>();
                _player.playerCanMove = false;
            }
        }
    }

    private IEnumerator RemoveScript(float length)
    {
        yield return new WaitForSeconds(length);
        Destroy(this);
        _player.playerCanMove = true;
    }
}
