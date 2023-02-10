using UnityEngine;

public class IgnitedObject : MonoBehaviour
{
    [SerializeField] private float _maxHealth = 10.0f;
    [SerializeField] private float _fireDuration = 5.0f;
    [SerializeField] private float _healthDecreaseRate = 1.0f;


    [SerializeField] private float _currentHealth;
    [SerializeField] private float _timeSinceIgnition;
    [SerializeField] private bool _isOnFire;

    private Animator _animator;

    private void Start()
    {
        _currentHealth = _maxHealth;
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_isOnFire)
        {
            _timeSinceIgnition += Time.deltaTime;
            _currentHealth -= Time.deltaTime * _healthDecreaseRate;

            if (_currentHealth <= 0)
            {
                _isOnFire = false;
                _animator.SetBool("isBurnt", true);
            }
        }
    }

    public void Ignite()
    {
        _isOnFire = true;
        _timeSinceIgnition = 0.0f;
    }

    public void Extinguish()
    {
        _isOnFire = false;
        _currentHealth -= _timeSinceIgnition * _healthDecreaseRate;
    }

}