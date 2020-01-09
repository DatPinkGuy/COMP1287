using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingInfo : MonoBehaviour
{
    [Header("Building Information")]
    public float neededAmount = 100f;
    public float currentAmount = 0f;
    public float buildSpeed = 10f;
    public bool placed = false;
    public Collider ObjectCollider => GetComponent<Collider>();
    public Transform ParentTransform => transform.parent;
    public Transform ThisTransform => gameObject.transform;
    private bool Built => neededAmount <= currentAmount;
    [HideInInspector] public List<AgentCharacters> agents;
    [HideInInspector] public List<AgentCharacters> buildingAgents;

    private void Start()
    {
        agents.AddRange(FindObjectsOfType<AgentCharacters>());
    }

    private void Update()
    {
        CheckBuild();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (Built) return;
        foreach (var agent in agents)
        {
            if (other.gameObject != agent.gameObject) continue;
            buildingAgents.Add(agent);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (Built) return;
        foreach (var agent in agents)
        {
            if (other.gameObject != agent.gameObject) continue;
            buildingAgents.Remove(agent);
        }
    }

    private void CheckBuild()
    {
        if (Built) return;
        StartCoroutine(BuildingProcess());
    }

    private void UseAgentEnergy()
    {
        foreach (var agent in buildingAgents)
        {
            agent.UseEnergy();
        }
    }
    
    private IEnumerator BuildingProcess()
    {
        UseAgentEnergy();
        currentAmount += (buildSpeed*Time.deltaTime) * buildingAgents.Count;
        yield return null;
    }
    
}
