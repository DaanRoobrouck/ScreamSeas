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
    [SerializeField] private bool _death;

    private AudioSource _audioSource;
    private PlayableDirector _director;

    private FirstPersonAIO _player;
    private UIManager _uiManager;

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
        if (_death)
        {
            _uiManager = (UIManager)FindObjectOfType(typeof(UIManager));
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
            else if (_death)
            {
                _uiManager.Dead();
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
        if (_player != null)
        {
            _player.playerCanMove = true;
        }       
        Destroy(this);
    }
}
