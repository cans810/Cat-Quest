using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenCanvasController : MonoBehaviour
{
    public Cat Player;

    public GameObject YouDiedObject;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Cat").GetComponent<Cat>();
        YouDiedObject = transform.Find("YouDied").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Player.isAlive){
            YouDiedObject.SetActive(true);
        }
    }

    public void YouDiedButton(){
        
    }
}
