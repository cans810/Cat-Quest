using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class CustomizableCharacter : MonoBehaviour
{
   public Animator animator;

   public List<RuntimeAnimatorController> animators;

   public void Awake(){
      animator = GetComponent<Animator>();
      animator.runtimeAnimatorController = animators[0];
      GameManager.Instance.currentCatSkin = animators[0];
      Debug.Log("")
   }

   public void Start(){
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