using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class EndTrigger : MonoBehaviour
{
    private PlayableDirector _director;
    private FirstPersonAIO _playerBehaviour;

    [SerializeField] private GameObject _cam;

    private void Start()
    {
        _director = GetComponent<PlayableDirector>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _cam.SetActive(true);

            _playerBehaviour = other.GetComponent<FirstPersonAIO>();
            _playerBehaviour.playerCanMove = false;
            _playerBehaviour.lockAndHideCursor = false;
            _playerBehaviour.enableCameraMovement = false;
            Rigidbody rb = _playerBehaviour.gameObject.GetComponent<Rigidbody>();
            _playerBehaviour.enabled = false;
            rb.velocity = Vector3.zero;
            rb.useGravity = false;

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            StartCoroutine(EnableMouse(110));
            _director.Play();
        }
    }

    private IEnumerator EnableMouse(float time)
    {
        yield return new WaitForSeconds(time);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        _director.Pause();
    }
}
