using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerNameText;

    public void SetPlayerName(string name)
    {
        playerNameText.text = name;
    }
}
