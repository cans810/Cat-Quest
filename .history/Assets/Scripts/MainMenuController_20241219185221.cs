using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGameButton(){
        screenFader.FadeToLevel("Level1Scene");
    }

    public void LevelsButton(){
        screenFader.FadeToLevel("LevelsMenuScene");
    }

    public void QuitButton(){
        
    }
}
