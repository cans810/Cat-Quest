using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenCanvasController : MonoBehaviour
{
    public GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Cat");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
