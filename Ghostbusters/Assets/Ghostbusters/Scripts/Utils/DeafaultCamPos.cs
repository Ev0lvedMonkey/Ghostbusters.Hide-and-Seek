using System.Collections;
using DG.Tweening;
using UnityEngine;

public class DeafaultCamPos : MonoBehaviour
{
    private const float MinZ = -0.8f;
    private const float MaxZ = 1f;

    private const float TweenDuration = 0.5f;
    private const float UpdateInterval = 2.5f;

    private Coroutine _limitCoroutine;
    private Tween _moveTween;

    private void Start()
    {
        _limitCoroutine = StartCoroutine(LimitZPositionCoroutine());
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

    private IEnumerator LimitZPositionCoroutine()
    {
        WaitForSeconds delay = new WaitForSeconds(UpdateInterval);

        while (true)
        {
            float clampedZ = Mathf.Clamp(transform.localPosition.z, MinZ, MaxZ);
            float clampedY = Mathf.Clamp(transform.transform.localPosition.y, 2.5f, 3.1f);
            Vector3 targetPosition = new Vector3(transform.localPosition.x, clampedY, clampedZ);

            _moveTween = transform.DOLocalMove(targetPosition, TweenDuration);

            yield return delay;
        }
    }
}