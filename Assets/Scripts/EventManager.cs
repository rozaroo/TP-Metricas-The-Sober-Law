using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public bool dontDestroyOnLoad;
    public static EventManager Instance;
    AudioSource pauseNoise;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            if (dontDestroyOnLoad) DontDestroyOnLoad(gameObject);
            else Destroy(gameObject);
        }
    }
}
