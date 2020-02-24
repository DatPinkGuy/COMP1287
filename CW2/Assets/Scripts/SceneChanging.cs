using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanging : MonoBehaviour
{
    private MeshRenderer MeshRenderer => GetComponent<MeshRenderer>();
    private static bool _sceneLoading;
    private BuildingAndMovementScript _mainScript;
    private Renderer FadeMaterial => fadeObject.GetComponent<Renderer>();
    [SerializeField] private GameObject fadeObject;
    [SerializeField] private Material[] materials;
    [SerializeField] private int levelNumber;

    private void Start()
    {
        _mainScript = FindObjectOfType<BuildingAndMovementScript>();
        MeshRenderer.material = materials[0];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_sceneLoading) return;
        StartCoroutine(ChangeLevel());
    }

    private IEnumerator ChangeLevel()
    {
        var fadeMaterialColor = FadeMaterial.material.color;
        _sceneLoading = true;
        MeshRenderer.material = materials[1];
        yield return new WaitForSeconds(2);
//        fadeMaterialColor.a += 0.01f * Time.deltaTime;
//        Debug.Log(fadeMaterialColor);
//        FadeMaterial.material.color = fadeMaterialColor;
        //yield return new WaitUntil(()=>fadeMaterialColor.a >= 1);
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(levelNumber);
        enabled = false;
    }
}
