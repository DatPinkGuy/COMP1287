using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodGather : MonoBehaviour
{
    public float neededAmount = 50f;
    public float currentAmount;
    public float buildSpeed = 10f;
    public float energyUse;
    public int woodAmount;
    [HideInInspector] public List<AgentCharacters> agents;
    [HideInInspector] public List<AgentCharacters> buildingAgents;
    private Watch _watchScript;
    private AudioSource _audioSource;
    private float _soundTimer;
    // Start is called before the first frame update
    void Start()
    {
        agents.AddRange(FindObjectsOfType<AgentCharacters>());
        _watchScript = FindObjectOfType<Watch>();
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
        
        foreach (var agent in buildingAgents)
        {
            agent.changeAnimation = agent.BuildingAnimation;
            agent.changeAnimation();
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
            agent.energy -= energyUse * Time.deltaTime;
        }
    }

    private void CheckIfDone()
    {
        if (currentAmount >= neededAmount)
        {
            _watchScript.woodCount += woodAmount;
            _watchScript.UpdateWood();
            foreach (var agent in buildingAgents)
            {
                agent.changeAnimation = agent.ResetBuildingAnimation;
                agent.changeAnimation();
            }
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
