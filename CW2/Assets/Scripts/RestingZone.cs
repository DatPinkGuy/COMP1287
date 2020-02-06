using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestingZone : MonoBehaviour
{
    private BuildingAndMovementScript _mainScript;
    [HideInInspector] public List<AgentCharacters> agents;
    [HideInInspector] public List<AgentCharacters> restingAgents;
    // Start is called before the first frame update
    void Start()
    {
        agents.AddRange(FindObjectsOfType<AgentCharacters>());
        _mainScript = FindObjectOfType<BuildingAndMovementScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        foreach (var agent in agents)
        {
            if (other.gameObject != agent.gameObject) continue;
            restingAgents.Add(agent);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        foreach (var agent in agents)
        {
            if (other.gameObject != agent.gameObject) continue;
            restingAgents.Remove(agent);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_mainScript.cycle != BuildingAndMovementScript.Cycle.Night) return;
        foreach (var agent in agents)
        {
            agent.energy += agent.energyUsage * Time.deltaTime;
        }
    }
}
