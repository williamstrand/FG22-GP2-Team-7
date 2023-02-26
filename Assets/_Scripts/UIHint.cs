using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIHint : MonoBehaviour
{
    public Animator animator;
    GameObject child;
    AudioSource audioSource;
    private void Start()
    {
        child = this.transform.GetChild(0).gameObject;
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            audioSource.Play();
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
