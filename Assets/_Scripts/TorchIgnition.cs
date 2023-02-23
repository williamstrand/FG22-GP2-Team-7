using System;
using UnityEngine;
using UnityEngine.VFX;

public class TorchIgnition : MonoBehaviour
{
    public Action OnTorchLit;
    public Action OnTorchPutOut;
    [SerializeField] private VisualEffect _fire;
    [SerializeField] private string _ignitionSourceTag = "IgnitionSource";

    private bool _isInsideIgnitionSourceTrigger = false;
    public bool _isTorchLit = false;

    private void Start()
    {
        _fire.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_ignitionSourceTag))
        {
            _isInsideIgnitionSourceTrigger = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_isInsideIgnitionSourceTrigger && Input.GetKeyDown(KeyCode.Keypad2))
        {
            IgniteTorch();
        }
    }

    public void IgniteTorch()
    {
        _isTorchLit = true;
        _fire.gameObject.SetActive(true);
        Debug.Log("Torch is now lit.");
        OnTorchLit?.Invoke();
    }

    public void PutOutTorchFire()
    {
        _isTorchLit = false;
        _fire.gameObject.SetActive(false);
        Debug.Log("Torch fire put out.");
        OnTorchPutOut?.Invoke();
    }
}