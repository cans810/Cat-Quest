using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    private float startPos, length;
    public GameObject cam; // Camera reference
    public float parallaxEffect; // Parallax speed multiplier
    public float autoScrollSpeed = 1.0f; // Speed for automatic scrolling

    private bool isAutoScrolling = false; 

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

        if (isAutoScrolling)
        {
            AutoScrollBackground();
        }
    }

    public void StartAutoScrolling()
    {
        isAutoScrolling = true;
    }

    public void StopAutoScrolling()
    {
        isAutoScrolling = false;
    }

    private void AutoScrollBackground()
    {
        startPos += autoScrollSpeed * Time.deltaTime;

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
