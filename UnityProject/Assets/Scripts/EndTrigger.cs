using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class EndTrigger : MonoBehaviour
{
    private PlayableDirector _director;
    private FirstPersonAIO _playerBehaviour;

    [SerializeField] private Camera _cam;

    private void Start()
    {
        _director = GetComponent<PlayableDirector>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerBehaviour = other.GetComponent<FirstPersonAIO>();
            _playerBehaviour.playerCanMove = false;
            _playerBehaviour.lockAndHideCursor = false;
            _playerBehaviour.enableCameraMovement = false;

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            StartCoroutine(EnableMouse(6));
            _director.Play();
        }
    }

    private IEnumerator EnableMouse(int time)
    {
        yield return new WaitForSeconds(time);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
}
