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
    private MeshRenderer ObjectMaterial => gameObject.GetComponent<MeshRenderer>();
    public bool Built => neededAmount <= currentAmount;
    public Transform EndPoint => endPoint.transform;
    [SerializeField] private Material[] materials;
    [SerializeField] private GameObject endPoint;
    [HideInInspector] public List<AgentCharacters> agents;
    [HideInInspector] public List<AgentCharacters> buildingAgents;
    [HideInInspector] public AgentCharacters builtAgent;

    private void Start()
    {
        ObjectMaterial.material = materials[0];
        agents.AddRange(FindObjectsOfType<AgentCharacters>());
    }

    private void Update()
    {
        CheckBuild();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (Built)
        {
            builtAgent = buildingAgents.Find(agent => agent.gameObject == other.gameObject);
        }
        else
        {
            foreach (var agent in agents)
            {
                if (other.gameObject != agent.gameObject) continue;
                buildingAgents.Add(agent);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (Built)
        {
            builtAgent = null;
        }
        else
        {
            foreach (var agent in agents)
            {
                if (other.gameObject != agent.gameObject) continue;
                buildingAgents.Remove(agent);
            }
        }
        
    }

    private void CheckBuild()
    {
        if (Built)
        {
            ObjectMaterial.material = materials[0];
            return;
        }
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

    public void MaterialChange()
    {
        ObjectMaterial.material = materials[1];
    }
}
