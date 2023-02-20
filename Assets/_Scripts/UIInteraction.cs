using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIInteraction : MonoBehaviour
{
    public Animator animator;
    GameObject child;
    private void Start()
    {
        child = this.transform.GetChild(0).gameObject;
    }
    //UI look at camera
    private void LateUpdate()
    {
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            child.SetActive(true);
            animator.SetTrigger("UIFadeIn");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            animator.SetTrigger("UIFadeOut");
        }
    }
    public void UIFadeOutComplete()
    {
        child.SetActive(false);
    }
}
