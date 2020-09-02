using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private WaypointManager _waypointManager;
    [SerializeField] private FirstPersonAIO _player;

    [SerializeField] private GameObject _deadPanel;

    public void Dead()
    {
        //UI
        _deadPanel.SetActive(true);

        //Player
        _player.transform.position = _waypointManager.ActiveWaypoint.position;
        _player.transform.rotation = _waypointManager.ActiveWaypoint.rotation;

        _player.enableCameraMovement = false;
        _player.playerCanMove = false;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Respawn()
    {
        //UI
        _deadPanel.SetActive(false);

        //Player


        _player.enableCameraMovement = true;
        _player.playerCanMove = true;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
