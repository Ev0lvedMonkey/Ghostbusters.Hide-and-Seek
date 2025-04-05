using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class ChatMessage : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _messageText;

    public void SetText(string message)
    { 
        _messageText.text = message; 
    }
}
