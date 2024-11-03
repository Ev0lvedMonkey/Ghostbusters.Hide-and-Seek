using UnityEngine;

public class GhostbusterMover : CharacterMover
{
    [SerializeField] private Animator _animator;

    private void Update()
    {
        if (!IsOwner) return;

        SetAnimState();
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;
        
        base.Move();
    }

    private void SetAnimState()
    {
        _animator.SetFloat(Horizontal, _input.x);
        _animator.SetFloat(Vertical, _input.y);
    }

}