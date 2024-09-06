
using UnityEngine;

public class TorseRotation : IRotatable
{
    private readonly Transform _torseObj;
    
    public TorseRotation(Transform torseObj)
    {
        _torseObj = torseObj;
    }

    public void Rotate(float xAxis, float yAxis)
    {
        _torseObj.localEulerAngles = new Vector3(yAxis, _torseObj.localEulerAngles.y, _torseObj.localEulerAngles.z);

    }
}


