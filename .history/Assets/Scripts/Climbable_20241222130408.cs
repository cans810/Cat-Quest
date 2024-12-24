using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbable : MonoBehaviour 
{
    public bool isOnLadder = false;
    public Rigidbody2D playerRigidbody;
    public float climbingSpeed = 4f;
    public float liftOffset = 0.1f;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isOnLadder = true;
            playerRigidbody = collision.GetComponent<Rigidbody2D>();
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isOnLadder = false;
            playerRigidbody = null;
        }
    }
}