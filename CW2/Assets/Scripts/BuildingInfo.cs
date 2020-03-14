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
    public float currentAmount;
    public float buildSpeed = 10f;
    public float energyUse;
    public Collider ObjectCollider => GetComponent<Collider>();
    public Transform ParentTransform => transform.parent;
    public Transform ThisTransform => gameObject.transform;
    private MeshRenderer ObjectMaterial => gameObject.GetComponent<MeshRenderer>();
    public bool Built => neededAmount <= currentAmount;
    private OffMeshLink OffMeshLink => GetComponent<OffMeshLink>();
    private Watch _watchScript;
    private AudioSource _audioSource;
    private float _soundTimer;
    private int _currentAmountInt;
    [SerializeField] private int neededWood;
    [SerializeField] private Material[] materials;
    [HideInInspector] public List<AgentCharacters> agents;
    [HideInInspector] public List<AgentCharacters> buildingAgents;


    private void Start()
    {
        ObjectMaterial.material = materials[0];
        agents.AddRange(FindObjectsOfType<AgentCharacters>());
        OffMeshLink.enabled = false;
        _watchScript = FindObjectOfType<Watch>();
        _audioSource = GetComponent<AudioSource>();
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
        foreach (var agent in buildingAgents.ToList())
        {
            if (other.gameObject != agent.gameObject) continue;
            buildingAgents.Remove(agent);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (Built) return;
        if (buildingAgents.Count <= 0) return;
        if (_watchScript.woodCount <= 0) return;
        _soundTimer += Time.deltaTime;
        if (_soundTimer > 1f)
        {
            _audioSource.Play();
            _soundTimer = 0;
        }

        foreach (var agent in buildingAgents.ToList())
        {
            agent.changeAnimation = agent.BuildingAnimation;
            agent.changeAnimation();
        }
    }

    private void CheckBuild()
    {
        if (Built)
        {
            ObjectMaterial.material = materials[0];
            OffMeshLink.enabled = true;
            return;
        }
        if (buildingAgents.Count == 0) return;
        if (_watchScript.woodCount > 0)
        {
            BuildingProcess();
        }
    }

    private void UseAgentEnergy()
    {
        foreach (var agent in buildingAgents)
        {
            agent.energy -= energyUse * Time.deltaTime;
        }
    }
    
    private void BuildingProcess()
    {
        UseAgentEnergy();
        currentAmount += (buildSpeed*Time.deltaTime) * buildingAgents.Count;
        var percentage = neededWood / neededAmount * currentAmount;
        var percentageInt = (int) percentage;
        if (_currentAmountInt == percentageInt) return;
        Debug.Log("Percentage: "  + percentage);
        Debug.Log("PercentageInt: "  +percentageInt);
        _currentAmountInt = percentageInt;
        _watchScript.woodCount -= 1;
        _watchScript.UpdateWood();
    }

    public void MaterialChange()
    {
        ObjectMaterial.material = materials[1];
    }

    private void CheckAgents()
    {
        if (buildingAgents.Count == 0) return;
        if (Built)
        {
            foreach (var agent in buildingAgents.ToList())
            {
                agent.changeAnimation = agent.ResetBuildingAnimation;
                agent.changeAnimation();
            }
            return;
        }
        foreach (var agent in buildingAgents.ToList())
        {
            if (!agent.gameObject.activeSelf) buildingAgents.Remove(agent);
        }
    }
}
