using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private WaypointManager _waypointManager;
    [SerializeField] private FirstPersonAIO _player;

    [SerializeField] private GameObject _deadPanel;
    [SerializeField] private Text _talkingText;
    [SerializeField] private GameObject _talkingPanel;

    [SerializeField] private Text _glowstickCountText;
    [SerializeField] private GameObject _glowstickCountPanel;
    [SerializeField] private GameObject _glowstickLight;
    [SerializeField] private float _glowStickTimer = 15f;

    private int _glowstickCount = 5;
    private float _timer;
    private bool _canUseGlowstick = true;
    private bool _glowStickInUse = false;

    [SerializeField] private PlayableDirector _countDirector;

    [SerializeField] private float _showCounterTime = 3;
    private Color _countColor;

    private AudioSource _audioSource;
    [SerializeField] private AudioClip _deadClip;
    [SerializeField] private AudioClip _glowStickClip;

    private void Start()
    {
        _countColor = _glowstickCountText.color;
        _audioSource = this.GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (_canUseGlowstick & _glowstickCount > 0 & Input.GetKeyDown(KeyCode.E))
        {
            _canUseGlowstick = false;
            _glowStickInUse = true;
            _glowstickCount--;
            _glowstickLight.SetActive(true);
            _timer = 0;
            ShowGlowstickAmount(_glowstickCount);
        }

        if (_glowStickInUse)
        {
            _timer += Time.deltaTime;
            if (_timer >= _glowStickTimer)
            {
                _glowStickInUse = false;
                _timer = 0;

                _glowstickLight.SetActive(false);
            }
        }
    }

    public void Dead()
    {
        //UI
        _deadPanel.SetActive(true);

        //Sound
        _audioSource.PlayOneShot(_deadClip);

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

    public void ShowText(string text)
    {
        _talkingText.text = text;
        _talkingPanel.SetActive(true);
    }

    public void HideText()
    {
        _talkingPanel.SetActive(false);
    }

    public void ShowGlowstickAmount(int amount)
    {
        //Sound
        _audioSource.PlayOneShot(_glowStickClip);

        _glowstickCountText.text = amount.ToString();
        _glowstickCountText.color = _countColor;
        _glowstickCountPanel.SetActive(true);

        StartCoroutine(HideGlowstickAmount(_showCounterTime));
    }

    public IEnumerator HideGlowstickAmount(float time)
    {
        yield return new WaitForSeconds(time);
        _countDirector.Play();
        StartCoroutine(CanUseGlowstick((float)_countDirector.duration));
    }

    private IEnumerator CanUseGlowstick(float duration)
    {
        yield return new WaitForSeconds(duration);
        _canUseGlowstick = true;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
