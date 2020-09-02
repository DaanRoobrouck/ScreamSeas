using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    private WaypointManager _manager;
    void Start()
    {
        _manager = GetComponentInParent<WaypointManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _manager.ActiveWaypoint = this.transform;
        }
    }
}
