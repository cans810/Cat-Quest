using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCatClickable : MonoBehaviour
{
    public CustomizableCharacter customizable;
    public int currentSkinIndex;

    public void Awake(){
        currentSkinIndex = 0;

        HandleClick(){}
    }

    private void OnMouseDown()
    {
        HandleClick();
    }

    private void HandleClick()
    {
        if (currentSkinIndex < customizable.animators.Count - 1)
        {
            currentSkinIndex++;
            customizable.ChangeController(currentSkinIndex);
        }
        else
        {
            currentSkinIndex = 0;
            customizable.ChangeController(currentSkinIndex);
        }
    }

}
