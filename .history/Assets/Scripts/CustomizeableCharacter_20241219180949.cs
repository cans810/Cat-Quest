using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class CustomizableCharacter : MonoBehaviour
{
   public Animator animator;

   public List<RuntimeAnimatorController> animators;

    public void ChangeController(int index)
    {
        if (animator != null && index != null)
        {
            animator.runtimeAnimatorController = newController;
        }
    }
}