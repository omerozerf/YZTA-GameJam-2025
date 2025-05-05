using UnityEngine;
using UnityEngine.UI;

public class OpenLinkButton : MonoBehaviour
{
    [SerializeField] private string _url;

    private void Awake()
    {
        var button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OpenURL);
        }
    }

    private void OpenURL()
    {
        if (!string.IsNullOrEmpty(_url))
        {
            Application.OpenURL(_url);
        }
    }

    private void OnDestroy()
    {
        var button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.RemoveListener(OpenURL);
        }
    }
}