using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropOffZone : MonoBehaviour
{
    public Action<Pushable> OnDropOff;
    
    private void OnTriggerStay(Collider other)
    {
        if (!other.TryGetComponent(out Pushable pushable)) return;
        
        OnDropOff?.Invoke(pushable);
    }
}
