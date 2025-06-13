using System.Collections;
using DG.Tweening;
using UnityEngine;

public class DeafaultCamPos : MonoBehaviour
{
    private const float TweenDuration = 0.5f;
    private const float UpdateInterval = 1f;

    private const float MinZ = -2.5f;
    private const float MaxZ = 0.1f;
    private const float MinY = 0f;
    private const float MaxY = 10f;

    [SerializeField] private Transform followedObject;
    [SerializeField] private Collider transformableObjectCollider;

    private Coroutine _limitCoroutine;
    private Tween _moveTween;

    private void Start()
    {
        _limitCoroutine = StartCoroutine(UpdatePositionCoroutine());
    }

    private void OnDisable()
    {
        if (_limitCoroutine != null)
        {
            StopCoroutine(_limitCoroutine);
            _limitCoroutine = null;
        }

        _moveTween?.Kill();
    }

    public void SetDeafaultStats()
    {
        transform.localEulerAngles = Vector3.zero;
    }

    private IEnumerator UpdatePositionCoroutine()
    {
        WaitForSeconds delay = new WaitForSeconds(UpdateInterval);

        while (true)
        {
            UpdateTargetPosition();
            yield return delay;
        }
    }

    private void UpdateTargetPosition()
    {
        if (followedObject == null)
            return;

        float heightOffset = GetObjectHeight() * 1.4f;
        float targetY = Mathf.Clamp(followedObject.position.y + heightOffset, MinY, MaxY);
        float targetZ = Mathf.Clamp(transform.localPosition.z, MinZ, MaxZ);

        Vector3 targetPos = new Vector3(transform.localPosition.x, targetY, targetZ);
        _moveTween = transform.DOLocalMove(targetPos, TweenDuration);
    }

    private float GetObjectHeight()
    {
        return transformableObjectCollider.bounds.size.y;
    }

    public void SetTransformedXCamPosition()
    {
        Vector3 targetPos = new Vector3(-1.6f, transform.localPosition.y, transform.localPosition.z);
        _moveTween = transform.DOLocalMove(targetPos, TweenDuration);
    }
}