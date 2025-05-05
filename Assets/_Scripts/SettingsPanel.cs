using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts
{
    public class SettingsPanel : MonoBehaviour
    {
        [SerializeField] private GameObject _settingsPanel;
        [SerializeField] private Slider _volumeSlider;
        [SerializeField] private AudioSource _audioSource;

        private static SettingsPanel ms_Instance;

        private void Awake()
        {
            ms_Instance = this;
            
            _settingsPanel.SetActive(false);
            DontDestroyOnLoad(this);
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Hide();
            }
        }
        
        public static void Show()
        {
            if (ms_Instance == null) return;

            if (ms_Instance._settingsPanel.activeSelf)
            {
                ms_Instance._settingsPanel.SetActive(false);
                return;
            }
            
            ms_Instance._settingsPanel.SetActive(true);
        }
        
        public static void Hide()
        {
            if (ms_Instance == null) return;
            ms_Instance._settingsPanel.SetActive(false);
        }
        
        private void Start()
        {
            if (_volumeSlider != null)
            {
                _volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
            }
            
            if (_audioSource != null)
            {
                _volumeSlider.value = LevelManager.GetAudioSource().volume;
            }
        }
        
        private void OnVolumeChanged(float value)
        {
            LevelManager.GetAudioSource().volume = value;
        }
    }
}