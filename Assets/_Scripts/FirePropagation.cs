using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class FirePropagation : MonoBehaviour
{
    [Tooltip("A list of VisualEffect objects representing the fire effects to propagate.")]
    [SerializeField] private List<VisualEffect> _fireEffects = new List<VisualEffect>();
    
    [Tooltip("The intensity of the fire, represented as a float.")]
    [SerializeField] private float _fireIntensity = 1.0f;
    
    private bool fireStarted = false;
    private float fireLerp = 0.0f;
    
    [Tooltip("The speed at which the fire grows and diminishes.")]
    [SerializeField] private float fireLerpSpeed = 0.1f;

    [Tooltip("Determines whether the fire should be started automatically at the beginning of the scene.")]
    [SerializeField] private bool startFire = false;

    private void Awake()
    {
        if (_fireEffects.Count == 0)
        {
            Debug.LogError("No Visual Effects are set in FirePropagation script.");
        }
        else
        {
            foreach (var effect in _fireEffects)
            {
                effect.SetFloat("FlameSize", 0);
                effect.gameObject.SetActive(false); // set the effect to inactive
            }
        }
    }

    public void IgniteFire()
    {
        if (_fireEffects.Count == 0)
        {
            Debug.LogError("No Visual Effects are set in FirePropagation script.");
        }
        else
        {
            fireStarted = true;
            foreach (var effect in _fireEffects)
            {
                effect.gameObject.SetActive(true); // activate the effect
                effect.Play();
            }
        }
    }

    public void PutOutFire()
    {
        fireStarted = false;
    }

    private void Update()
    {
        if (_fireEffects.Count == 0) return;

        if (startFire)
        {
            startFire = false;
            IgniteFire();
        }

        if (fireStarted)
        {
            fireLerp = Mathf.MoveTowards(fireLerp, 1.0f, fireLerpSpeed * Time.deltaTime);
            foreach (var effect in _fireEffects)
            {
                effect.SetFloat("FlameSize", Mathf.Lerp(0, _fireIntensity, fireLerp));
            }
        }
        else
        {
            fireLerp = Mathf.MoveTowards(fireLerp, 0.0f, fireLerpSpeed * Time.deltaTime);
            foreach (var effect in _fireEffects)
            {
                effect.SetFloat("FlameSize", Mathf.Lerp(_fireIntensity, 0, fireLerp));
                if (effect.GetFloat("FlameSize") <= 0.1f)
                {
                    effect.Stop();
                    effect.gameObject.SetActive(false); // deactivate the effect
                }
            }
        }
    }
}
