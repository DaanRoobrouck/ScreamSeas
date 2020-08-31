using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Playables;
using System;

public class Navigator : MonoBehaviour
{
    [SerializeField] private GameObject _parentPanel;
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _optionsPanel;

    [SerializeField] private AudioMixer _ambientSoundMixer;
    [SerializeField] private AudioMixer _soundEffectsMixer;

    [SerializeField] private Slider _ambientSoundSlider;
    [SerializeField] private Slider _soundEffectsSlider;

    [SerializeField] private PlayableDirector _director;

    private GameManager _gameManager;
    private void Start()
    {
        _gameManager = (GameManager)FindObjectOfType(typeof(GameManager));
    }

    public void Play()
    {
        _parentPanel.SetActive(false);
        _director.Play();

        StartCoroutine(LoadGame((float)_director.duration));
    }

    private IEnumerator LoadGame(float duration)
    {
        yield return new WaitForSeconds(duration);
        SceneManager.LoadScene(1);
    }

    public void Options()
    {
        _menuPanel.SetActive(false);
        _optionsPanel.SetActive(true);

        _ambientSoundSlider.value = _gameManager.AmbientSoundVolume;
        _soundEffectsSlider.value = _gameManager.SfxVolume;
    }

    public void UpdateAmbientAudio()
    {
        float volume = Mathf.Log10(_ambientSoundSlider.value) * 20;
        _ambientSoundMixer.SetFloat("AmbientSound", volume);
        _gameManager.AmbientSoundVolume = _ambientSoundSlider.value;
    }

    public void UpdateSoundEffectsAudio()
    {
        float volume = Mathf.Log10(_soundEffectsSlider.value) * 20;
        _soundEffectsMixer.SetFloat("SoundEffects", volume);
        _gameManager.SfxVolume = _soundEffectsSlider.value;
    }

    public void Back()
    {
        _menuPanel.SetActive(true);
        _optionsPanel.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
