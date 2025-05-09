using _Scripts;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class QuitButton : MonoBehaviour
    {
        private Button m_QuitButton;
        
        
        private void Awake()
        {
            m_QuitButton = GetComponent<Button>();
            m_QuitButton.onClick.AddListener(OnQuitButtonClicked);
        }

        private void OnQuitButtonClicked()
        {
            LevelManager.PlayButtonClickSound();
            LevelManager.Instance.PlaySceneTransition(0).Forget();
        }
        
        private void OnDestroy()
        {
            m_QuitButton.onClick.RemoveListener(OnQuitButtonClicked);
        }
    }
}