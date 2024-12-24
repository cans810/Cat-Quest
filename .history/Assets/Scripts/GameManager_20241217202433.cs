using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public static GameManager Instance { get; private set; }

    public BackgroundController mainMenuBackground1;
    public BackgroundController mainMenuBackground2;
    public BackgroundController mainMenuBackground3;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        mainMenuBackground1.StartAutoScrolling();
        mainMenuBackground2.StartAutoScrolling();
        mainMenuBackground3.StartAutoScrolling();

        mainMenuBackground1.ChangeScrollDirection();
        mainMenuBackground2.ChangeScrollDirection();
        mainMenuBackground3.ChangeScrollDirection();
    }

    void Update()
    {
    }

    public void StartGameButton(){

    }

    public void LevelsButton(){
        
    }

    public void StartGameButton(){
        
    }
}
