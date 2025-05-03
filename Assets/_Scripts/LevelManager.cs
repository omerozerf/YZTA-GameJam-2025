using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;

namespace _Scripts
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { get; private set; }

        [SerializeField] private CanvasGroup transitionCanvasGroup;
        [SerializeField] private RectTransform transitionImage;
        [SerializeField] private float transitionDuration = 0.5f;
        [SerializeField] private TMP_Text levelNameText;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            if (levelNameText != null)
            {
                levelNameText.text = SceneManager.GetActiveScene().name.ToUpperInvariant();
            }
        }

        private async UniTask PlaySceneTransition(int sceneIndex)
        {
            transitionCanvasGroup.alpha = 1;
            transitionImage.localScale = Vector3.zero;
            transitionCanvasGroup.blocksRaycasts = true;

            await transitionImage.DOScale(Vector3.one, transitionDuration).SetEase(Ease.InQuad).AsyncWaitForCompletion();

            await SceneManager.LoadSceneAsync(sceneIndex);
            
            if (levelNameText != null)
            {
                levelNameText.text = SceneManager.GetActiveScene().name.ToUpperInvariant();
            }

            transitionImage.localScale = Vector3.one;
            await transitionImage.DOScale(Vector3.zero, transitionDuration).SetEase(Ease.OutQuad).AsyncWaitForCompletion();
            

            transitionCanvasGroup.alpha = 0;
            transitionCanvasGroup.blocksRaycasts = false;
        }

        public static void LoadNextLevel()
        {
            int currentIndex = SceneManager.GetActiveScene().buildIndex;
            int nextIndex = currentIndex + 1;

            if (nextIndex < SceneManager.sceneCountInBuildSettings)
            {
                Instance.PlaySceneTransition(nextIndex).Forget();
            }
            else
            {
                Debug.Log("No more levels to load.");
                Instance.PlaySceneTransition(0).Forget();
            }
        }
    }
}