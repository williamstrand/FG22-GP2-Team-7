using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PearlEndMenu : MonoBehaviour
{
    public int scene;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == ("Player"))
        {
            UIManager.instance.FadeToScene(scene);
        }
    }
}
