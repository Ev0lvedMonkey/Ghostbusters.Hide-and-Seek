using UnityEngine;

public class FullBodyRotation : IRotatable
{
    private readonly Transform _characterTransform;

    public FullBodyRotation(Transform characterTransform)
    {
        _characterTransform = characterTransform;
    }

    public void Rotate(float xAxis, float yAxis)
    {
        _characterTransform.eulerAngles = new Vector3(_characterTransform.eulerAngles.x, xAxis, _characterTransform.eulerAngles.z);
    }
}
