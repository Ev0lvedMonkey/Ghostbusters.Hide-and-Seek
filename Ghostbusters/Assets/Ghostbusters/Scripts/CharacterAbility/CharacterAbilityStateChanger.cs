using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class CharacterAbilityStateChanger : MonoBehaviour
{
    private readonly Color EnabledColor = Color.green;
    private readonly Color DisabledColor = Color.gray;

    [SerializeField] private Image _abilityImage;

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
