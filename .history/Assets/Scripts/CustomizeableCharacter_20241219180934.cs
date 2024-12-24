using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class CustomizableCharacter : MonoBehaviour
{
   public Animator currentAnimator;

   public List<RuntimeAnimatorController> animators;

   public Animator animator; // Reference to the Animator component

    void ChangeController()
    {
        if (animator != null && newController != null)
        {
            animator.runtimeAnimatorController = newController;
        }
    }
}