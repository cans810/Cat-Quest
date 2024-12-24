using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelButton : MonoBehaviour
{
    public string levelSceneName;

    public ScreenFader screenFader;


    // Start is called before the first frame update
    void Start()
    {
        screenFader = GameObject.Find("ScreenFader").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToLevel(){
        screenFader.FadeToLevel("Level1Scene");
    }
}
