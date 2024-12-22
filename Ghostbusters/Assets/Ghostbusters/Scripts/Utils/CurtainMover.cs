using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurtainMover : MonoBehaviour
{
    [SerializeField] private List<Transform> _curtainsParts;
    [SerializeField] private bool _isSecretRoomCurtain;

    private void Start()
    {
        if(_isSecretRoomCurtain)
            return;
        GameStateManager.Instance.OnStartGame.AddListener(Open);
    }

    private void Open()
    {
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
    }
}
