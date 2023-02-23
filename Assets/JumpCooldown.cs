using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCooldown : MonoBehaviour
{
    public void JumpFinished()
    {
        GetComponentInParent<WaterCharacterController>().JumpFinished();
    }
        
}
