using UnityEngine;
using UnityEngine.UI;

public abstract class HelpUI : MonoBehaviour
{
    [Header("Base Components")]
    [SerializeField] private Button _exitButton;

    public virtual void Init()
    {
        _exitButton.onClick.AddListener(Hide);
        Hide();
    }

    public virtual void Show()
    {
         gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
