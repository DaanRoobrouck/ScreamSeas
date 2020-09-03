using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkingBehaviour : MonoBehaviour
{
    [SerializeField] private string _talkingText;

    private UIManager _uiManager;

    private void Start()
    {
        _uiManager = (UIManager)FindObjectOfType(typeof(UIManager));
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _uiManager.ShowText(_talkingText);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _uiManager.HideText();
        }
    }
}
