using UnityEngine;
using UnityEngine.UI;

public class HelpUI : MonoBehaviour
{
    [SerializeField] private Button _exitButton;

    public void Init()
    {
        _exitButton.onClick.AddListener(Hide);
        Hide();
    }

    public void Show() =>
         gameObject.SetActive(true);

    public void Hide() =>
        gameObject.SetActive(false);
}
