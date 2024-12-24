using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class CustomizableCharacter : MonoBehaviour
{
   public Animator animator;

   public List<RuntimeAnimatorController> animators;

    void ChangeController()
    {
        if (animator != null && newController != null)
        {
            animator.runtimeAnimatorController = newController;
        }
    }
}