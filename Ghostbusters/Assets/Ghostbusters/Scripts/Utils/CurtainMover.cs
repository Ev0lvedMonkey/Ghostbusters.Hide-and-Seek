using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class CurtainMover : MonoBehaviour
{
    [SerializeField] private List<Transform> _curtainsParts;
    [SerializeField] private bool _isSecretRoomCurtain;
    [SerializeField] private AudioSource _audioSource;

    private bool _isMade;
    private GameStateManager _gameStateManager;

    private void Start()
    {
        _gameStateManager = ServiceLocator.Current.Get<GameStateManager>();

        if (_isSecretRoomCurtain)
        {
            _gameStateManager.OnSecretRoomOpen.AddListener(Open);
            return;
        }
        _gameStateManager.OnStartGame.AddListener(Open);
    }

    private void Open()
    {
        if (_isMade)
            return;
        if(_isSecretRoomCurtain)
            _audioSource.Play();
        Sequence curtainSequence = DOTween.Sequence();
        Vector3 finalTargetPosition;
        for (int i = 0; i <= _curtainsParts.Count; i++)
        {
            if (i == _curtainsParts.Count - 1)
            {
                Transform finalCurtainPart = _curtainsParts[i];
                BoxCollider collider = finalCurtainPart.GetComponent<BoxCollider>();
                finalTargetPosition =
                    new(finalCurtainPart.position.x, collider.bounds.max.y, finalCurtainPart.position.z);
                curtainSequence.Append(_curtainsParts[i].DOMove(finalTargetPosition, 0.5f));
                curtainSequence.Append(_curtainsParts[i].DOScale(0, 0.01f));
                break;
            }
            curtainSequence.Append(_curtainsParts[i].DOMove(_curtainsParts[i + 1].position, 0.5f));
            curtainSequence.Append(_curtainsParts[i].DOScale(0, 0.01f));
        }
        _isMade = true;
    }
}
