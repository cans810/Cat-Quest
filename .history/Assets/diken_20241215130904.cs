using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class diken : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("kedi"))
        {
            YourFunction(collision.gameObject); 
        }
    }

    private void YourFunction(GameObject gameobject)
    {
        gameobject.GetComponent<Animator>().SetBool("dead",true);
        gameobject.GetComponent<Cat>().enabled = false;
            }
}


