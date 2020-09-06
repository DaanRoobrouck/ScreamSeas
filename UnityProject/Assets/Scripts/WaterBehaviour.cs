using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBehaviour : MonoBehaviour
{
    private AudioSource _audioSource;
    private void Start()
    {
        _audioSource = this.GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FirstPersonAIO player = other.GetComponent<FirstPersonAIO>();
            player.advanced.gravityMultiplier = 0.7f;
            //player.IsInWater = true;

            _audioSource.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FirstPersonAIO player = other.GetComponent<FirstPersonAIO>();
            player.advanced.gravityMultiplier = 1f;
            //player.IsInWater = false;
        }
    }
}
