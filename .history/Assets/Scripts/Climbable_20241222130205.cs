using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbable : MonoBehaviour 
{
    public bool isOnLadder = false;
    public Rigidbody2D playerRigidbody;
    public float climbingSpeed = 4f;
    public float liftOffset = 0.1f;
    
    
    void Update()
    {
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isOnLadder = true;
            playerRigidbody = collision.GetComponent<Rigidbody2D>();
            catScript = collision.GetComponent<Cat>();
            originalPlayerScale = collision.transform.localScale;
            ladderXPosition = transform.position.x;
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isOnLadder = false;
            playerRigidbody = null;
            catScript = null;
        }
    }
}