using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class CharacterAbilityStateChanger : MonoBehaviour
{
    [SerializeField] private Image _abilityImage;

    [SerializeField] private Color EnabledColor = Color.green;
    [SerializeField] private Color DisabledColor = Color.gray;

    private void OnValidate()
    {
        if (_abilityImage == null)
            _abilityImage = GetComponent<Image>();
    }

    private void OnEnable()
    {
        EnableAbility();
    }

    public void EnableAbility()
    {
        _abilityImage.color = EnabledColor;
    }

    public void DisableAbility()
    {
        _abilityImage.color = DisabledColor;
    }
}
