using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanging : MonoBehaviour
{
    public Action buttonPress;
    private static bool _sceneLoading;
    private Renderer FadeMaterial => fadeObject.GetComponent<Renderer>();
    private Color FadeMaterialColor
    {
        get => FadeMaterial.material.color;
        set => FadeMaterial.material.color = value;
    }
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject fadeObject;
    public bool SceneLoading
    {
        get => _sceneLoading;
        set => _sceneLoading = value;
    }


    private void Start()
    {
        if (_sceneLoading)
        {
            var fadeMaterialColor = FadeMaterial.material.color;
            fadeMaterialColor.a = 1;
            FadeMaterialColor = fadeMaterialColor;
            StartCoroutine(UndoFade());
        }
        else
        {
            fadeObject.SetActive(false);
        }
    }

    public IEnumerator ChangeLevel(SceneButton button)
    {
        if (_sceneLoading) yield break;
        _sceneLoading = true;
        button.MeshRenderer.material.EnableKeyword("_EMISSION");
        if(button.buttonClick) button.buttonClick.Play();
        fadeObject.SetActive(true);
        var fadeMaterialColor = FadeMaterialColor;
        yield return new WaitForSeconds(2);
        while (fadeMaterialColor.a < 1)
        {
            if (audioSource)
            {
                audioSource.volume -= 1f * Time.deltaTime;
            }
            fadeMaterialColor.a += 1f * Time.deltaTime;
            FadeMaterialColor = fadeMaterialColor;
            yield return null;
        }
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(button.levelNumber);
        enabled = false;
    }

    private IEnumerator UndoFade()
    {
        audioSource.Play();
        var fadeMaterialColor = FadeMaterialColor;
        yield return new WaitForSeconds(2);
        while (fadeMaterialColor.a > 0)
        {
            fadeMaterialColor.a -= 1f * Time.deltaTime;
           FadeMaterialColor = fadeMaterialColor;
            yield return null;
        }
        fadeObject.SetActive(false);
        _sceneLoading = false;
        yield return null;
    }
}
