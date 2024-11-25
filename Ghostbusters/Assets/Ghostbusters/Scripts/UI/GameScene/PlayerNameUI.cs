using Unity.Netcode;
using UnityEngine;

public class PlayerNameUI : NetworkBehaviour
{
    [SerializeField] private Canvas _canvas;

    private void OnValidate()
    {
        if (_canvas == null)
            _canvas = GetComponent<Canvas>();
    }

    private void Start()
    {
        if (IsOwner)
            _canvas.worldCamera = Camera.main;
        else
            gameObject.SetActive(false);
    }
}
