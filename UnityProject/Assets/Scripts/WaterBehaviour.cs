using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBehaviour : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FirstPersonAIO player = other.GetComponent<FirstPersonAIO>();
            player.advanced.gravityMultiplier = 0.2f;
            player.IsInWater = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FirstPersonAIO player = other.GetComponent<FirstPersonAIO>();
            player.advanced.gravityMultiplier = 1f;
            player.IsInWater = false;
        }
    }
}
