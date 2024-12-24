using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatHitBox : MonoBehaviour
{
    private List<GameObject> objectsInHitbox = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!objectsInHitbox.Contains(other.gameObject))
        {
            objectsInHitbox.Add(other.gameObject);
            Debug.Log(other.gameObject.name + " entered the hitbox.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (objectsInHitbox.Contains(other.gameObject))
        {
            objectsInHitbox.Remove(other.gameObject);
            Debug.Log(other.gameObject.name + " exited the hitbox.");
        }
    }

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
