using UnityEngine;

public class BusterHealthController : CharacterHealthController
{
    [Header("UI Elements")]
    [SerializeField] private WorldSpaceCanvasTransform _spaceCanvasTransform;

    protected override void BusterUniqMove()
    {
        _spaceCanvasTransform?.gameObject.SetActive(false);
    }
}