using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCatClickable : MonoBehaviour
{
    private void OnMouseDown()
    {
        HandleClick();
    }

    private void HandleClick()
    {
        GetComponent<SpriteRenderer>().color = new Color(
            Random.value,
            Random.value,
            Random.value
        );
    }
}
