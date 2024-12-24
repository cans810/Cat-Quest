using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCatClickable : MonoBehaviour
{
    public CustomizableCharacter customizable;
    public int currentSkinIndex;

    public void Start(){
        currentSkinIndex = 0;
    }

    private void OnMouseDown()
    {
        HandleClick();
    }

    private void HandleClick()
    {
        customizable.ChangeController(currentSkinIndex);
    }
}
