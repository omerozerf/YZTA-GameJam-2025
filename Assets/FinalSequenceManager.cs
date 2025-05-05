using _Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class FinalSequenceManager : MonoBehaviour
{
    [SerializeField] private Transform redBox;
    [SerializeField] private Transform blueBox;
    [SerializeField] private Image fadeImage;
    [SerializeField] private float duration = 2f;
    [SerializeField] private string finalSceneName = "MainMenu";

    private async void Start()
    {
        await UniTask.Delay(500);

        Vector3 midPoint = (redBox.position + blueBox.position) / 2;

        DOTween.Sequence()
            .Append(redBox.DOMove(midPoint, duration).SetEase(Ease.InOutSine))
            .Join(blueBox.DOMove(midPoint, duration).SetEase(Ease.InOutSine))
            .Join(redBox.DOScale(1.2f, duration * 0.5f).SetLoops(2, LoopType.Yoyo))
            .Join(blueBox.DOScale(1.2f, duration * 0.5f).SetLoops(2, LoopType.Yoyo))
            .OnComplete(async () =>
            {
                await fadeImage.DOFade(1, 1f).AsyncWaitForCompletion();
                LevelManager.LoadNextLevel();
            });
    }
}
