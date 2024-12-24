using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCatClickable : MonoBehaviour
{
    private void OnMouseDown()
    {
        HandleClick();
    }

    // Custom method to handle the click action
    private void HandleClick()
    {
        // Example action: Change the object's color to a random one
        GetComponent<SpriteRenderer>().color = new Color(
            Random.value,
            Random.value,
            Random.value
        );
    }
}
