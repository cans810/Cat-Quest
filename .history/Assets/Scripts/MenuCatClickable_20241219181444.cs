using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCatClickable : MonoBehaviour
{
    // This function is called when the object is clicked
    private void OnMouseDown()
    {
        // Log a message to the console
        Debug.Log($"{gameObject.name} was clicked!");

        // Perform your desired action here
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
