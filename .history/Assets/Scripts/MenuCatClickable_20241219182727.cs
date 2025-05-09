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
        if (currentSkinIndex < customizable.animators.Count - 1)
        {
            // Increment index and change the controller
            currentSkinIndex++;
            customizable.ChangeController(currentSkinIndex);
        }
        else
        {
            // Reset to the first skin when reaching the last
            currentSkinIndex = 0;
            customizable.ChangeController(currentSkinIndex);
        }
    }

}
