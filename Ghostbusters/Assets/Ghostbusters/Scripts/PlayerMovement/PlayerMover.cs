using UnityEngine;

public class GhostBusterMover : CharacterMover
{
    [SerializeField] private Animator _animator;

    private void Update()
    {
        SetAnimState();
        base.Move();
    }

    private void SetAnimState()
    {
        _animator.SetFloat(Horizontal, _input.x);
        _animator.SetFloat(Vertical, _input.y);
    }

}
