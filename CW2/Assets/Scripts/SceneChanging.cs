using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanging : MonoBehaviour
{
    private MeshRenderer MeshRenderer => GetComponent<MeshRenderer>();
    private static bool _sceneLoading;
    private Renderer FadeMaterial => fadeObject.GetComponent<Renderer>();
    [SerializeField] private GameObject fadeObject;
    [SerializeField] private Material[] materials;
    [SerializeField] private int levelNumber;

    private void Start()
    {
        if (_sceneLoading)
        {
            var fadeMaterialColor = FadeMaterial.material.color;
            fadeMaterialColor.a = 1;
            FadeMaterial.material.color = fadeMaterialColor;
            StartCoroutine(UndoFade());
        }
        else
        {
            fadeObject.SetActive(false);
        }
        if(MeshRenderer) MeshRenderer.material = materials[0];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_sceneLoading) return;
        StartCoroutine(ChangeLevel());
    }

    private IEnumerator ChangeLevel()
    {
        fadeObject.SetActive(true);
        var fadeMaterialColor = FadeMaterial.material.color;
        _sceneLoading = true;
        MeshRenderer.material = materials[1];
        yield return new WaitForSeconds(2);
        while (fadeMaterialColor.a < 1)
        {
            fadeMaterialColor.a += 1f * Time.deltaTime;
            FadeMaterial.material.color = fadeMaterialColor;
            yield return null;
        }
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(levelNumber);
        enabled = false;
    }

    private IEnumerator UndoFade()
    {
        var fadeMaterialColor = FadeMaterial.material.color;
        yield return new WaitForSeconds(2);
        while (fadeMaterialColor.a > 0)
        {
            fadeMaterialColor.a -= 1f * Time.deltaTime;
            FadeMaterial.material.color = fadeMaterialColor;
            yield return null;
        }
        fadeObject.SetActive(false);
    }
}
