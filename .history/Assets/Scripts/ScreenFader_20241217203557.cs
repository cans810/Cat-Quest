using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader Instance { get; private set; } // Singleton instance

    public Animator animator;

    private string levelToLoad;

    void Awake()
    {
        // Singleton pattern - ensure only one instance exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    public void FadeToLevel(string levelName)
    {
        levelToLoad = levelName;
        animator.SetTrigger("FadeOut");
    }

    // Called at the end of the fade animation
    public void OnFadeComplete()
    {
        SceneManager.LoadScene(levelToLoad);
    }
}
