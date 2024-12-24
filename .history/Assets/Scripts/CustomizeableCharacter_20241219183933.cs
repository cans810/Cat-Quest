using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class CustomizableCharacter : MonoBehaviour
{
   public Animator animator;

   public List<RuntimeAnimatorController> animators;

   public void Start(){
      animator = GetComponent<Animator>();   
 }

    public void ChangeController(int index)
    {
        if (animator != null && animators[index] != null)
        {
            animator.runtimeAnimatorController = animators[index];
            GameManager.Instance.currentCatSkin = animators[index];
        }
    }
}