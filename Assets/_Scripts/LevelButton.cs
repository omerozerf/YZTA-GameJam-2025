using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Scripts
{
    public class LevelButton : MonoBehaviour
    {
        [FormerlySerializedAs("_level")] [SerializeField] private int _levelIndex;
        [SerializeField] private Image _lockedImage;
        
        
        private void Awake()
        {
            var button = GetComponent<UnityEngine.UI.Button>();
            button.onClick.AddListener(OnLevelButtonClicked);
        }
        
        private void OnLevelButtonClicked()
        {
            if (_lockedImage.gameObject.activeSelf)
            {
                LevelManager.PlayButtonClickSound();
                return;
            }
            
            LevelPanel.Hide();
            
            LevelManager.PlayButtonClickSound();
            _ = LevelManager.Instance.PlaySceneTransition(_levelIndex);
        }
        
        private void OnDestroy()
        {
            var button = GetComponent<UnityEngine.UI.Button>();
            button.onClick.RemoveListener(OnLevelButtonClicked);
        }  
        
        public int GetLevelIndex()
        {
            return _levelIndex;
        }
        
        public void SetActiveLockedImage(bool isActive)
        {
            _lockedImage.gameObject.SetActive(isActive);
        }
    }
}