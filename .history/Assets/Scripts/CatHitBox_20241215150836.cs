using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatHitBox : MonoBehaviour
{
    private List<GameObject> objectsInHitbox = new List<GameObject>();

    // Called when another collider enters the trigger collider of the hitbox
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object is not already in the list
        if (!objectsInHitbox.Contains(other.gameObject))
        {
            // Add the object to the list
            objectsInHitbox.Add(other.gameObject);
            Debug.Log(other.gameObject.name + " entered the hitbox.");
        }
    }

    // Called when another collider exits the trigger collider of the hitbox
    private void OnTriggerExit2D(Collider2D other)
    {
        // Remove the object from the list
        if (objectsInHitbox.Contains(other.gameObject))
        {
            objectsInHitbox.Remove(other.gameObject);
            Debug.Log(other.gameObject.name + " exited the hitbox.");
        }
    }

    // You can use this function to get all objects inside the hitbox
    public List<GameObject> GetObjectsInHitbox()
    {
        return objectsInHitbox;
    }

    // Optionally, you can check the objects in the list during each frame (in the Update method)
    void Update()
    {
        foreach (GameObject obj in objectsInHitbox)
        {
            // Perform actions on objects inside the hitbox (for example, print their name)
            Debug.Log("Object in hitbox: " + obj.name);
        }
    }
}
