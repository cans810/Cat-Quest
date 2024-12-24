using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCatClickable : MonoBehaviour
{
    public CustomizableCharacter customizable;
    public int currentSkinIndex;

    public void 

    private void OnMouseDown()
    {
        HandleClick();
    }

    private void HandleClick()
    {
        customizable.ChangeController(currentSkinIndex);
    }
}
