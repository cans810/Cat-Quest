using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Static reference to the singleton instance
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        // Ensure only one instance exists
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
