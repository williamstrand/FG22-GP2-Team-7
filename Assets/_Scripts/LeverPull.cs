using UnityEngine;
using UnityEngine.Events;

public class LeverPull : MonoBehaviour, IInteractable
{
   private Animator _animator;
   public UnityEvent onPull;
   private void Awake()
   {
      _animator = GetComponent<Animator>();
   }

   public void Pull()
   {
      _animator.Play("LeverPull");
      onPull?.Invoke();
   }
}