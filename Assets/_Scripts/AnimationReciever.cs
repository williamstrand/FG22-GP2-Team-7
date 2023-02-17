using UnityEngine;

public class AnimationReciever : MonoBehaviour
{
    private Animator _animator;
    public string animationName;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    public void PlayAnimation()
    {
        _animator.Play(animationName);
    }
}