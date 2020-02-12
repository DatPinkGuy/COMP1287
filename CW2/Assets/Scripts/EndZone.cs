using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndZone : MonoBehaviour
{
    [SerializeField] private int requiredAgents;
    [SerializeField] private BuildingAndMovementScript mainScript;
    [HideInInspector] public List<AgentCharacters> agents;
    [HideInInspector] public List<AgentCharacters> agentsInside;
    // Start is called before the first frame update
    void Start()
    {
        agents.AddRange(FindObjectsOfType<AgentCharacters>());
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
            agentsInside.Add(agent);
        }

        if (requiredAgents == agentsInside.Count)
        {
            Debug.Log("won");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        foreach (var agent in agentsInside)
        {
            if (other.gameObject != agent.gameObject) continue;
            agentsInside.Remove(agent);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        foreach (var agent in agents)
        {
            agent.health += agent.healthUsage * Time.deltaTime;
        }
    }
}
