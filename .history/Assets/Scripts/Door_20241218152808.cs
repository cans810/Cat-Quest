using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public string SceneToFade;

    public GAM

    // Start is called before the first frame update
    void Start()
    {
        GameObject 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) 
        {
           FadeToScreen();
        }
    }

    public void FadeToScreen(){
        screenFader.FadeToLevel(SceneToFade);
    }
}
