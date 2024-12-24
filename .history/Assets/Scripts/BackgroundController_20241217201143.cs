using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    private float startPos, length;
    public GameObject cam; // Camera reference
    public float parallaxEffect; // Parallax speed multiplier
    public float autoScrollSpeed = 1.0f; // Speed for automatic scrolling

    private bool isAutoScrolling = false; // Flag to enable auto-scrolling

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void LateUpdate() // Use LateUpdate for smoother movement
    {
        // Background movement based on camera
        float distance = cam.transform.position.x * parallaxEffect;
        float movement = cam.transform.position.x * (1 - parallaxEffect);

        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

        if (movement > startPos + length)
        {
            startPos += length;
        }
        else if (movement < startPos - length)
        {
            startPos -= length;
        }

        // Automatic background movement
        if (isAutoScrolling)
        {
            AutoScrollBackground();
        }
    }

    // Enable automatic background scrolling
    public void StartAutoScrolling()
    {
        isAutoScrolling = true;
    }

    // Stop automatic background scrolling
    public void StopAutoScrolling()
    {
        isAutoScrolling = false;
    }

    // Auto-scroll method for background movement
    private void AutoScrollBackground()
    {
        // Simulate movement by adjusting startPos
        startPos += autoScrollSpeed * Time.deltaTime;

        // Reset position when out of bounds
        if (startPos > length)
        {
            startPos -= length;
        }
        else if (startPos < -length)
        {
            startPos += length;
        }
    }
}
