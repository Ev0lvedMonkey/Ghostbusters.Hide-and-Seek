using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeZone : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out CharacterHealthControllerTemp component))
        {      
            component.MomentKillClientRpc();
        }
    }
}