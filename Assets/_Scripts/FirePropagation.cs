using UnityEngine;

public class FirePropagation : MonoBehaviour
{
    [SerializeField] private ParticleSystem _fireParticles;
    [SerializeField] private float _fireIntensity = 1.0f;
    private float fireEmission;
    private bool fireStarted = false;
    private float fireLerp = 0.0f;
    private float fireLerpSpeed = 0.1f;
    
    private void Start()
    {
        fireEmission = _fireParticles.emission.rateOverTime.constant;
        var fireParticlesEmission = _fireParticles.emission;
        fireParticlesEmission.rateOverTime = 0;
    }

    public void IgniteFire()
    {
        if (_fireParticles != null)
        {
            fireStarted = true;
            _fireParticles.Play();
        }
        else
        {
            Debug.LogError("Particle System is not set in FirePropagation script.");
        }
    }

    public void PutOutFire()
    {
        fireStarted = false;
    }

    private void Update()
    {
        var fireParticlesEmission = _fireParticles.emission;
        
        if (fireStarted)
        {
            fireLerp = Mathf.MoveTowards(fireLerp, 1.0f, fireLerpSpeed * Time.deltaTime);
            fireParticlesEmission.rateOverTime = Mathf.Lerp(0, fireEmission, fireLerp);
            _fireIntensity = Mathf.Lerp(0, 1.0f, fireLerp);
        }
        else
        {
            fireLerp = Mathf.MoveTowards(fireLerp, 0.0f, fireLerpSpeed * Time.deltaTime);
            fireParticlesEmission.rateOverTime = Mathf.Lerp(fireParticlesEmission.rateOverTime.constant, 0, fireLerp);
            _fireIntensity = Mathf.Lerp(_fireIntensity, 0, fireLerp);
            if (_fireIntensity <= 0.1f)
            {
                _fireParticles.Stop();
            }
        }
    }
}