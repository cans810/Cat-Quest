using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instance
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Initialization logic
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
