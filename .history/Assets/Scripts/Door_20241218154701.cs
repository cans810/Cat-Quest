using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public string SceneToFade;
    public ScreenFader screenFader;
    private bool playerInRange = false;

    void Start()
    {
        screenFader = GameObject.Find("ScreenFader").GetComponent<ScreenFader>();
    }

    void Update()
    {
        // Check if the player is within range and presses E
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            FadeToScreen();
        }
    }

    public void FadeToScreen()
    {
        screenFader.FadeToLevel(SceneToFade);
    }

    // Detect when the player enters the collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Ensure only the player triggers the event
        {
            playerInRange = true;
            Debug.Log("Player entered door range");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            playerInRange = false;
            Debug.Log("Player exited door range");
        }
    }
}
