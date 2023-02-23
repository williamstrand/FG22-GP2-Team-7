using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PearlEndMenu : MonoBehaviour
{
    public UIManager uIManager;
    public int scene;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == ("Player"))
        {
            uIManager.FadeToScene(scene);
        }
    }
}
