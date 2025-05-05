using UnityEngine;
using UnityEngine.UI;

namespace _Scripts
{
    public class MyLevelButton : MonoBehaviour
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
            LevelPanel.Show();
        }
    
        private void OnDestroy()
        {
            m_Button.onClick.RemoveListener(OnSettingsButtonClicked);
        }
    }
}