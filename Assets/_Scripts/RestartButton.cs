using _Scripts;
using UnityEngine;
using UnityEngine.UI;

public class RestartButton : MonoBehaviour
{
    private Button m_RestartButton;
    
    
    private void Awake()
    {
        m_RestartButton = GetComponent<Button>();
        m_RestartButton.onClick.AddListener(OnRestartButtonClicked);
    }
    
    private void OnRestartButtonClicked()
    {
        LevelManager.PlayButtonClickSound();
        LevelManager.RestartLevel();
    }
    
    private void OnDestroy()
    {
        m_RestartButton.onClick.RemoveListener(OnRestartButtonClicked);
    }
}
