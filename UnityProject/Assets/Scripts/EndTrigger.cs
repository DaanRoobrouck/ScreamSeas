using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class EndTrigger : MonoBehaviour
{
    private bool _moveCam = false;
    private PlayableDirector _director;
    [SerializeField] private Vector3 _offset;
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

            //_moveCam = true;

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            StartCoroutine(EnableMouse(6));
            _director.Play();
        }
    }

    private void Update()
    {
        //if (_moveCam)
        //{

        //    _cam.transform.position = Vector3.Lerp(_cam.transform.position, transform.position + _offset, Time.deltaTime * 1);
        //    _cam.transform.rotation = Quaternion.Lerp(_cam.transform.rotation, transform.rotation, Time.deltaTime * 5);

        //    if (Vector3.Distance(_cam.transform.position, transform.position + _offset) <= 0.1f)
        //    {
        //        _moveCam = false;
        //        StartCoroutine(EnableMouse(6));
        //        _director.Play();
        //    }
        //}
    }

    private IEnumerator EnableMouse(int time)
    {
        yield return new WaitForSeconds(time);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
}
