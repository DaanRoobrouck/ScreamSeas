using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private bool _canWalk;
    [SerializeField] Transform[] _waypoints;

    [SerializeField] private float _movementSpeed = 5f;
    [SerializeField] private float _waitTime = 5f;
    [SerializeField] private float _fieldOfViewAngle = 120f;
    [SerializeField] private float _detectRange = 3f;
    [SerializeField] private FirstPersonAIO _player;

    private Transform _gotoWaypoint;
    private int _currentIndex = 1;

    private bool _hasLooked = false;
    private bool _walkForwards = true;

    private bool _foundPlayer = false;

    private UIManager _uiManager;

    void Start()
    {
        _gotoWaypoint = _waypoints[1];
        _uiManager = (UIManager)FindObjectOfType(typeof(UIManager));
    }

    void Update()
    {
        if (_foundPlayer)
        {
            WalkToPlayer();
        }
        else
        {
            CheckForPlayer();
            if (!_hasLooked)
            {
                LookAtNextWaypoint();
            }
            else if (_canWalk)
            {
                WalkToNextWaypoint();
            }
        }
     
    }

    private void WalkToPlayer()
    {
        transform.position = Vector3.Lerp(transform.position, _gotoWaypoint.position, _movementSpeed * Time.deltaTime);
        transform.LookAt(_player.transform);

        if (Vector3.Distance(transform.position, _gotoWaypoint.position) < 0.1f)
        {
            _uiManager.Dead();
        }
    }

    private void CheckForPlayer()
    {
        Vector3 direction = _player.transform.position - transform.position;
        float angle = Vector3.Angle(direction, transform.forward);
        float distance = Vector3.Distance(transform.position, _player.transform.position);

        if (_player.Targetable & angle <= _fieldOfViewAngle * 0.5f && distance <= _detectRange)
        {
            _foundPlayer = true;
        }
    }

    private void WalkToNextWaypoint()
    {
        transform.position = Vector3.Lerp(transform.position, _gotoWaypoint.position, _movementSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, _gotoWaypoint.position) < 0.1f)
        {
            if (_gotoWaypoint = _waypoints[_waypoints.Length])
            {
                _walkForwards = false;
            }
            else if (_gotoWaypoint = _waypoints[0])
            {
                _walkForwards = true;
            }

            if (_walkForwards)
            {
                _currentIndex++;
                _gotoWaypoint = _waypoints[_currentIndex];
            }
            else
            {
                _currentIndex--;
                _gotoWaypoint = _waypoints[_currentIndex];
            }

            _hasLooked = false;
        }
    }

    private void LookAtNextWaypoint()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, _gotoWaypoint.rotation, _movementSpeed * Time.deltaTime);

        StartCoroutine(StartMoving());
    }

    private IEnumerator StartMoving()
    {
        yield return new WaitForSeconds(_waitTime);
        _hasLooked = true;
        _canWalk = true;
    }
}
