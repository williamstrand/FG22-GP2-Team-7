using UnityEngine;
using UnityEngine.VFX;

public class FirePropagation : MonoBehaviour
{
    [SerializeField] private VisualEffect _fireEffect;
    [SerializeField] private float _fireIntensity = 1.0f;
    private bool fireStarted = false;
    private float fireLerp = 0.0f;
    private float fireLerpSpeed = 0.1f;

    private void Start()
    {
        if (_fireEffect == null)
        {
            Debug.LogError("Visual Effect is not set in FirePropagation script.");
        }
        else
        {
            _fireEffect.SetFloat("Intensity", 0);
        }
    }

    public void IgniteFire()
    {
        if (_fireEffect != null)
        {
            fireStarted = true;
            _fireEffect.Play();
        }
        else
        {
            Debug.LogError("Visual Effect is not set in FirePropagation script.");
        }
    }

    public void PutOutFire()
    {
        fireStarted = false;
    }

    private void Update()
    {
        if (_fireEffect == null) return;

        if (fireStarted)
        {
            fireLerp = Mathf.MoveTowards(fireLerp, 1.0f, fireLerpSpeed * Time.deltaTime);
            _fireEffect.SetFloat("Intensity", Mathf.Lerp(0, _fireIntensity, fireLerp));
        }
        else
        {
            fireLerp = Mathf.MoveTowards(fireLerp, 0.0f, fireLerpSpeed * Time.deltaTime);
            _fireEffect.SetFloat("Intensity", Mathf.Lerp(_fireIntensity, 0, fireLerp));
            if (_fireEffect.GetFloat("Intensity") <= 0.1f)
            {
                _fireEffect.Stop();
            }
        }
    }
}