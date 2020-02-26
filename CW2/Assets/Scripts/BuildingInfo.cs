using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class BuildingInfo : MonoBehaviour
{
    [Header("Building Information")]
    public float neededAmount = 100f;
    public float currentAmount = 0f;
    public float buildSpeed = 10f;
    public Collider ObjectCollider => GetComponent<Collider>();
    public Transform ParentTransform => transform.parent;
    public Transform ThisTransform => gameObject.transform;
    private MeshRenderer ObjectMaterial => gameObject.GetComponent<MeshRenderer>();
    public bool Built => neededAmount <= currentAmount;
    private OffMeshLink OffMeshLink => GetComponent<OffMeshLink>();
    private BuildingAndMovementScript _mainScript;
    private bool _removedWood;
    [SerializeField] private int neededWood;
    [SerializeField] private Material[] materials;
    [HideInInspector] public List<AgentCharacters> agents;
     public List<AgentCharacters> buildingAgents;


    private void Start()
    {
        ObjectMaterial.material = materials[0];
        agents.AddRange(FindObjectsOfType<AgentCharacters>());
        OffMeshLink.enabled = false;
        _mainScript = FindObjectOfType<BuildingAndMovementScript>();
    }

    private void Update()
    {
        if (!ObjectCollider.enabled) 
        {
            buildingAgents.Clear();
            return;
        }
        CheckAgents();
        CheckBuild();
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
        foreach (var agent in buildingAgents)
        {
            if (other.gameObject != agent.gameObject) continue;
            buildingAgents.Remove(agent);
        }
    }

    private void CheckBuild()
    {
        if (Built)
        {
            RemoveWood();
            ObjectMaterial.material = materials[0];
            OffMeshLink.enabled = true;
            return;
        }
        if (buildingAgents.Count == 0) return;
        if (!_removedWood && _mainScript.woodCount >= neededWood)
        {
            BuildingProcess();
        }
        
    }

    private void UseAgentEnergy()
    {
        foreach (var agent in buildingAgents)
        {
            agent.UseEnergy();
        }
    }
    
    private void BuildingProcess()
    {
        UseAgentEnergy();
        currentAmount += (buildSpeed*Time.deltaTime) * buildingAgents.Count;
    }

    public void MaterialChange()
    {
        ObjectMaterial.material = materials[1];
    }

    private void RemoveWood()
    {
        if (_removedWood) return;
        _mainScript.woodCount -= neededWood;
        _mainScript.UpdateWood();
        _removedWood = true;
    }

    private void CheckAgents()
    {
        if (Built || buildingAgents.Count == 0) return;
        foreach (var agent in buildingAgents)
        {
            if (!agent.gameObject.activeSelf) buildingAgents.Remove(agent);
        }
    }
}
