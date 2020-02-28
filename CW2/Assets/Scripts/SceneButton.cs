using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButton : MonoBehaviour
{
    public int levelNumber;
    public Material[] materials;
    public MeshRenderer MeshRenderer => GetComponent<MeshRenderer>();
    [HideInInspector] public AudioSource buttonClick;
    private bool _levelAvailable = true;
    private SceneChanging _sceneChanging;
    // Start is called before the first frame update
    void Start()
    {
        _sceneChanging = FindObjectOfType<SceneChanging>();
        buttonClick = GetComponent<AudioSource>();
        if (SceneManager.GetActiveScene().buildIndex == levelNumber)
        {
            MeshRenderer.material = materials[1];
            _levelAvailable = false;
        }
        else
        {
            MeshRenderer.material = materials[0];
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        _sceneChanging.buttonPress = ButtonPress;
        _sceneChanging.buttonPress();
    }

    private void ButtonPress()
    {
        if (!_levelAvailable) return;
        StartCoroutine(_sceneChanging.ChangeLevel(this));
    }
}
