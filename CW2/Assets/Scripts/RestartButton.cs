using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    private SceneChanging _sceneChanging;

    private void Start()
    {
        _sceneChanging = FindObjectOfType<SceneChanging>();
    }
    public void ChangeLevelStart()
    {
        _sceneChanging.RestartButton();
    }
    
}
