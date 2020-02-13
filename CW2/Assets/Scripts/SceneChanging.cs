using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanging : MonoBehaviour
{
    private MeshRenderer MeshRenderer => GetComponent<MeshRenderer>();
    [SerializeField] private Material[] materials;
    [SerializeField] private int levelNumber;

    private void Start()
    {
        MeshRenderer.material = materials[0];
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(ChangeLevel());
    }

    private IEnumerator ChangeLevel()
    {
        MeshRenderer.material = materials[1];
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(levelNumber);
    }
}
