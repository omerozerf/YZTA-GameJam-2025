using System;
using _Scripts;
using UnityEngine;
using UnityEngine.UI;

public class SettingsButton : MonoBehaviour
{
    private Button m_Button;
    
    
    private void Awake()
    {
        m_Button = GetComponent<Button>();
        m_Button.onClick.AddListener(OnSettingsButtonClicked);
    }
    
    private void OnSettingsButtonClicked()
    {
        LevelManager.PlayButtonClickSound();
        SettingsPanel.Show();
    }
    
    private void OnDestroy()
    {
        m_Button.onClick.RemoveListener(OnSettingsButtonClicked);
    }
}
