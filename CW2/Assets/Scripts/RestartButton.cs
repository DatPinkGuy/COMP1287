using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    private SceneChanging _sceneChanging;
    private Renderer FadeMaterial => fadeObject.GetComponent<Renderer>();
    private Color FadeMaterialColor
    {
        get => FadeMaterial.material.color;
        set => FadeMaterial.material.color = value;
    }
    [SerializeField] private GameObject fadeObject;

    private void Start()
    {
        _sceneChanging = FindObjectOfType<SceneChanging>();
    }
    public void ChangeLevelStart()
    {
        StartCoroutine(ChangeLevel());
    }
    
    private IEnumerator ChangeLevel()
    {
        if (_sceneChanging.SceneLoading) yield break;
        _sceneChanging.SceneLoading = true;
        fadeObject.SetActive(true);
        var fadeMaterialColor = FadeMaterialColor;
        yield return new WaitForSeconds(2);
        while (fadeMaterialColor.a < 1)
        {
            fadeMaterialColor.a += 1f * Time.deltaTime;
            FadeMaterialColor = fadeMaterialColor;
            yield return null;
        }
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        enabled = false;
    }
}
