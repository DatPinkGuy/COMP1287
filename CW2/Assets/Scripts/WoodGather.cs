using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodGather : MonoBehaviour
{
    public float neededAmount = 50f;
    public float currentAmount = 0f;
    public float buildSpeed = 10f;
    public int woodAmount;
    [HideInInspector] public List<AgentCharacters> agents;
    [HideInInspector] public List<AgentCharacters> buildingAgents;
    private BuildingAndMovementScript _mainScript;
    private AudioSource _audioSource;
    private float _soundTimer;
    // Start is called before the first frame update
    void Start()
    {
        agents.AddRange(FindObjectsOfType<AgentCharacters>());
        _mainScript = FindObjectOfType<BuildingAndMovementScript>();
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckUse();
        CheckIfDone();
    }

    private void OnTriggerEnter(Collider other)
    {
        foreach (var agent in agents)
        {
            if (other.gameObject != agent.gameObject) continue;
            buildingAgents.Add(agent);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        foreach (var agent in agents)
        {
            if (other.gameObject != agent.gameObject) continue;
            buildingAgents.Remove(agent);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(buildingAgents.Count <= 0) return;
        _soundTimer += Time.deltaTime;
        if (_soundTimer > 1f)
        {
            _audioSource.Play();
            _soundTimer = 0;
        }
    }

    private void CheckUse()
    {
        StartCoroutine(CuttingProgress());
    }
    
    private void UseAgentEnergy()
    {
        foreach (var agent in buildingAgents)
        {
            agent.UseEnergy();
        }
    }

    private void CheckIfDone()
    {
        if (currentAmount >= neededAmount)
        {
            _mainScript.woodCount += woodAmount;
            _mainScript.UpdateWood();
            gameObject.SetActive(false);
        }
    }
    private IEnumerator CuttingProgress()
    {
        UseAgentEnergy();
        currentAmount += (buildSpeed*Time.deltaTime) * buildingAgents.Count;
        yield return null;
    }
}
