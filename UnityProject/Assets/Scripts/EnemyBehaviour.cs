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
    [SerializeField] private float _deadTime = 2f;
    [SerializeField] private float _fieldOfViewAngle = 120f;
    [SerializeField] private float _detectRange = 3f;
    [SerializeField] private FirstPersonAIO _player;
    [SerializeField] private Transform _enemy;

    private Transform _gotoWaypoint;
    private int _currentIndex = 1;

    private bool _hasLooked = false;
    private bool _walkForwards = true;

    private bool _foundPlayer = false;
    private bool _killedPlayer = false;

    private UIManager _uiManager;

    void Start()
    {
        _gotoWaypoint = _waypoints[1];
        _uiManager = (UIManager)FindObjectOfType(typeof(UIManager));
    }

    void Update()
    {
        if (!_killedPlayer)
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
    }

    private void WalkToPlayer()
    {
        _enemy.transform.position = Vector3.MoveTowards(_enemy.transform.position, _player.transform.position, _movementSpeed * Time.deltaTime);
        _enemy.transform.LookAt(_player.transform);

        if (!_player.Targetable)
        {
            _foundPlayer = false;
        }

        if (Vector3.Distance(_enemy.transform.position, _player.transform.position) < 1f)
        {
            _killedPlayer = true;
            _player.playerCanMove = false;
            _player.enableCameraMovement = false;
            _player.transform.LookAt(_enemy.transform.GetChild(0).transform);
            StartCoroutine(ShowDeadScreen());        
        }
    }

    private void ResetEnemy()
    {
        _foundPlayer = false;
        _enemy.transform.position = _waypoints[0].position;
        _enemy.transform.rotation = _waypoints[0].rotation;

        _currentIndex = 0;
        _gotoWaypoint = _waypoints[0];

        _killedPlayer = false;
    }

    private IEnumerator ShowDeadScreen()
    {
        yield return new WaitForSeconds(_deadTime);
        _uiManager.Dead();
        ResetEnemy();
    }

    private void CheckForPlayer()
    {
        Vector3 direction = _player.transform.position - _enemy.transform.position;
        float angle = Vector3.Angle(direction, _enemy.transform.forward);
        float distance = Vector3.Distance(_enemy.transform.position, _player.transform.position);

        if (_player.Targetable & angle <= _fieldOfViewAngle * 0.5f && distance <= _detectRange)
        {
            _foundPlayer = true;

            //play sound
        }
    }

    private void WalkToNextWaypoint()
    {
        _enemy.transform.position = Vector3.MoveTowards(_enemy.transform.position, _gotoWaypoint.position, _movementSpeed * Time.deltaTime);

        if (Vector3.Distance(_enemy.transform.position, _gotoWaypoint.position) < 0.1f)
        {
            if (_gotoWaypoint == _waypoints[_waypoints.Length - 1])
            {
                _walkForwards = false;
            }
            else if (_gotoWaypoint == _waypoints[0])
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
        _enemy.transform.LookAt(_gotoWaypoint.transform);

        StartCoroutine(StartMoving());
    }

    private IEnumerator StartMoving()
    {
        yield return new WaitForSeconds(_waitTime);
        _hasLooked = true;
        _canWalk = true;
    }
}
