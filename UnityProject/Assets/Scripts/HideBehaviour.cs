using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideBehaviour : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UpdatePlayerState(other, false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UpdatePlayerState(other, true);
        }
    }

    private void UpdatePlayerState(Collider other, bool state)
    {
        FirstPersonAIO player = other.GetComponent<FirstPersonAIO>();
        player.Targetable = state;
    }
}
