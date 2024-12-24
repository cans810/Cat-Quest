using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class CustomizableCharacter : MonoBehaviour
{
   public Animator currentAnimator;

   public List<Animator> animators;

   public void Start(){
    currentAnimator = GetComponent<Animator>();
   }

   public void changeSkin(int index){
    
   }
}