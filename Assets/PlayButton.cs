using _Scripts;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    private Button m_PlayButton;
    
    
    private void Awake()
    {
        m_PlayButton = GetComponent<Button>();
        m_PlayButton.onClick.AddListener(OnPlayButtonClicked);
    }
    
    private void OnPlayButtonClicked()
    {
        LevelManager.PlayButtonClickSound();
        LevelManager.LoadNextLevel();
    }
    
    private void OnDestroy()
    {
        m_PlayButton.onClick.RemoveListener(OnPlayButtonClicked);
    }
}
