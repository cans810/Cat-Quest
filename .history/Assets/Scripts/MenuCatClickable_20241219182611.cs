using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCatClickable : MonoBehaviour
{
    public CustomizableCharacter customizable;
    public int currentSkinIndex;

    public void Awake(){
        currentSkinIndex = 0;
    }

    private void OnMouseDown()
    {
        HandleClick();
    }

    private void HandleClick()
    {
        if (customizable.animators.Count >= currentSkinIndex){
            customizable.ChangeController(currentSkinIndex++);
        }
        else if(customizable.animators.Count >= currentSkinIndex){
            currentSkinIndex = 0;
            customizable.ChangeController(currentSkinIndex);
        }
    }
}
