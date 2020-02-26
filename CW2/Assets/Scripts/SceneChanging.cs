using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanging : MonoBehaviour
{
    private bool _levelAvailable = true;
    private static bool _sceneLoading;
    private MeshRenderer MeshRenderer => GetComponent<MeshRenderer>();
    private Renderer FadeMaterial => fadeObject.GetComponent<Renderer>();
    private Color FadeMaterialColor
    {
        get => FadeMaterial.material.color;
        set => FadeMaterial.material.color = value;
    }

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject fadeObject;
    [SerializeField] private Material[] materials;
    [SerializeField] private int levelNumber;

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == levelNumber)
        {
            MeshRenderer.material = materials[1];
            _levelAvailable = false;
        }
        else
        {
            MeshRenderer.material = materials[0];
        }
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

    private void OnTriggerEnter(Collider other)
    {
        if (_sceneLoading) return;
        if (!_levelAvailable) return;
        StartCoroutine(ChangeLevel());
    }

    private IEnumerator ChangeLevel()
    {
        MeshRenderer.material.EnableKeyword("_EMISSION");
        fadeObject.SetActive(true);
        var fadeMaterialColor = FadeMaterialColor;
        _sceneLoading = true;
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
        SceneManager.LoadScene(levelNumber);
        enabled = false;
    }

    private IEnumerator UndoFade()
    {
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
