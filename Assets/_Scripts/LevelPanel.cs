using System;
using UnityEngine;

namespace _Scripts
{
    public class LevelPanel : MonoBehaviour
    {
        [SerializeField] private GameObject _levelButtonPrefab;
        [SerializeField] private Transform _levelButtonContainer;
        [SerializeField] private LevelButton[] _levelButtons;
        
        private static LevelPanel ms_Instance;
        
        
        private void Awake()
        {
            ms_Instance = this;
            gameObject.SetActive(false);
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            int currentIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex - 2;

            for (int i = 0; i < _levelButtons.Length; i++)
            {
                bool isLocked = i > currentIndex;
                _levelButtons[i].SetActiveLockedImage(isLocked);
            }
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
            int currentIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex - 2;

            for (int i = 0; i < ms_Instance._levelButtons.Length; i++)
            {
                bool isLocked = i > currentIndex;
                ms_Instance._levelButtons[i].SetActiveLockedImage(isLocked);
            }
            
            if (ms_Instance.gameObject.activeSelf)
            {
                ms_Instance.gameObject.SetActive(false);
                return;
            }
            
            ms_Instance.gameObject.SetActive(true);
        }

        public static void Hide()
        {
            ms_Instance.gameObject.SetActive(false);
        }
    }
}