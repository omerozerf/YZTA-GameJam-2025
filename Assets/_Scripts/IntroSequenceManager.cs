using _Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class IntroSequenceManager : MonoBehaviour
{
    [SerializeField] private Transform redBox;
    [SerializeField] private Transform blueBox;
    [SerializeField] private Image fadeImage;
    [SerializeField] private float duration = 2f;
    [SerializeField] private string nextSceneName = "Level_01";

    private async void Start()
    {
        redBox.position = blueBox.position;

        // Yükselme efekti
        blueBox.DOMoveY(blueBox.position.y + 1f, 0.3f).SetEase(Ease.OutQuad);
        redBox.DOMoveY(redBox.position.y + 1f, 0.3f).SetEase(Ease.OutQuad);

        // Ses ve kamera zoom
        Camera.main.DOOrthoSize(6f, 2f).SetEase(Ease.InOutSine);
        var audio = Camera.main.GetComponent<AudioSource>();
        if (audio != null)
        {
            audio.Play();
        }

        await UniTask.Delay(1000);

        DOTween.Sequence()
            .Append(redBox.DORotate(new Vector3(0, 0, 360), duration, RotateMode.FastBeyond360).SetEase(Ease.InOutSine))
            .Join(blueBox.DORotate(new Vector3(0, 0, -360), duration, RotateMode.FastBeyond360).SetEase(Ease.InOutSine))
            .Join(redBox.DOMoveX(redBox.position.x - 2f, duration).SetEase(Ease.OutQuad))
            .Join(blueBox.DOMoveX(blueBox.position.x + 2f, duration).SetEase(Ease.OutQuad))
            .OnComplete(async () =>
            {
                await fadeImage.DOFade(1, 1f).AsyncWaitForCompletion();
                LevelManager.LoadNextLevel();
            });
    }
}