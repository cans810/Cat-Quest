using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public string SceneToFade;
    public ScreenFader screenFader;
    private bool playerInRange = false;

    public bool isLocked;

    void Start()
    {
        screenFader = GameObject.Find("ScreenFader").GetComponent<ScreenFader>();
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !isLocked)
        {
            FadeToScreen();
        }
        else if (playerInRange && Input.GetKeyDown(KeyCode.E) && isLocked ){

        }
    }

    public void FadeToScreen()
    {
        screenFader.FadeToLevel(SceneToFade);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
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
