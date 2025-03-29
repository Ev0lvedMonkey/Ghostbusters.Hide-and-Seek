using UnityEngine;
using TMPro;

public class ChatMessage : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI messageText;

    public void SetText(string message)
    { 
        messageText.text = message; 
    }
}
