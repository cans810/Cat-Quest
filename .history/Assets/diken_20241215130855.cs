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
            Debug.Log("Tilemap ile etkile�im ger�ekle�ti!");
            YourFunction(collision.gameObject); // Kendi fonksiyonunuzu �a��r�n
        }
    }

    private void YourFunction(GameObject gameobject)
    {
        // Burada kendi istedi�iniz i�lemleri yapabilirsiniz
        gameobject.GetComponent<Animator>().SetBool("dead",true);
        gameobject.GetComponent<Cat>().enabled = false;
        

        Debug.Log("Fonksiyon �al��t�!");
    }
}


