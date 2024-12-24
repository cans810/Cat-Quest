using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class CustomizableCharacter : MonoBehaviour
{
   public Animator currentAnimator;

   public List<Animator> animators;

   public Animator animator; // Reference to the Animator component
    public RuntimeAnimatorController newController; // New Animator Controller to switch to

    void ChangeController()
    {
        if (animator != null && newController != null)
        {
            animator.runtimeAnimatorController = newController;
        }
    }
}