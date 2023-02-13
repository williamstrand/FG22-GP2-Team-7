using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    [SerializeField] AudioClip hoverSound;
    [SerializeField] AudioClip pressedSound;

    public void hoverButton()
    {
        audioSource.PlayOneShot(hoverSound);
    }
    public void clickButton()
    {
        audioSource.PlayOneShot(pressedSound);
    }
}
