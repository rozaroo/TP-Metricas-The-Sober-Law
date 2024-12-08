using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level2UI : MonoBehaviour
{
    public LevelManager lvlmanager;

    void Start()
    {
        lvlmanager = GameObject.FindObjectOfType<LevelManager>();
        if (lvlmanager == null) Debug.LogError("No se encontró el LevelManager en la escena.");

    }
}
