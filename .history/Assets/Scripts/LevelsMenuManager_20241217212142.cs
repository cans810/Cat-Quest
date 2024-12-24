using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsMenuManager : MonoBehaviour
{
    public BackgroundController mainMenuBackground1;
    public BackgroundController mainMenuBackground2;
    public BackgroundController mainMenuBackground3;

    public ScreenFader screenFader;


    // Start is called before the first frame update
    void Start()
    {
        mainMenuBackground1.StartAutoScrolling();
        mainMenuBackground2.StartAutoScrolling();
        mainMenuBackground3.StartAutoScrolling();

        mainMenuBackground1.ChangeScrollDirection();
        mainMenuBackground2.ChangeScrollDirection();
        mainMenuBackground3.ChangeScrollDirection();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoBackButton()
    {
        screenFader.FadeToLevel("");
    }
}
