using System;
using System.Collections;
using System.Collections.Generic;
using OVRTouchSample;
using UnityEngine;
using UnityEngine.AI;

public class BuildingAndMovementScript : MonoBehaviour
{
    private RaycastHit _hit;
    private Ray _ray;
    private Collider _collider;
    private Transform _buildingParent;
    public BuildingInfo chosenBuilding;
    public NavMeshAgent currentAgent;
    private enum Cycle
    {
        Day,
        Night
    }
    private Cycle _cycle;
    private SunMoon _sunMoon;
    private Transform HandTransform => hand.transform;
    private int _layerMask = 1 << 8;
    [SerializeField] private Hand hand;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private List<NavMeshAgent> agents;
    [SerializeField] private List<AgentCharacters> agentCharacter;
    [SerializeField] private List<BuildingInfo> buildings;

    // Start is called before the first frame update
    void Start()
    {
        _layerMask = ~_layerMask;
        _sunMoon = FindObjectOfType<SunMoon>();
        agents.AddRange(FindObjectsOfType<NavMeshAgent>());
        agentCharacter.AddRange(FindObjectsOfType<AgentCharacters>());
        buildings.AddRange(FindObjectsOfType<BuildingInfo>());
        _cycle = Cycle.Day;
    }
    
    // Update is called once per frame
    void Update()
    {
        DrawRaycasts();
        BuildingCharacterLogic();
        if (OVRInput.GetDown(OVRInput.Button.Two)) DayNightSwitch();
        DayNightCycle();
        if (chosenBuilding)
        {
            MoveObjectToRaycast();
            PlaceObject();
        }
    }
    
    private void MoveObjectToRaycast()
    {
        _collider.enabled = false;
        if (Physics.Raycast(_ray, out _hit, 5, _layerMask))
        {
            _buildingParent.position = _hit.point;
        }
        else
        {
             _buildingParent.position = HandTransform.forward + HandTransform.position;
        }
        if (OVRInput.GetDown(OVRInput.Button.SecondaryThumbstickUp)) 
        { 
            _buildingParent.transform.Rotate(0,15,0);
        }
        else if (OVRInput.GetDown(OVRInput.Button.SecondaryThumbstickDown)) 
        { 
            _buildingParent.transform.Rotate(0, -15, 0);
        }
    }

    private void PlaceObject()
    {
        if (chosenBuilding && OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger))
        {
            _collider.enabled = true;
            chosenBuilding = null;
            _buildingParent = null;
        }
    }

    private void DayNightSwitch()
    {
        _cycle = _cycle == Cycle.Day ? Cycle.Night : Cycle.Day;
    }

    private void DayNightCycle()
    {
        switch (_cycle)
        {
            case Cycle.Day:
                foreach (var agent in agents)
                {
                    agent.enabled = true;
                }
                _sunMoon.ChangeToSun();
                break;
            case Cycle.Night:
                foreach (var agent in agentCharacter)
                {
                    agent.energy += agent.energyUsage * Time.deltaTime;
                    agent.health += agent.healthUsage * Time.deltaTime;
                }

                foreach (var agent in agents)
                {
                    agent.enabled = false;
                }
                _sunMoon.ChangeToMoon();
                break;
        }
    }

    private void BuildingCharacterLogic()
    {
        _ray = new Ray(HandTransform.position,HandTransform.forward);
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            if (Physics.Raycast(_ray, out _hit,5, _layerMask))
            {
                foreach (var building in buildings)
                {
                    if (building.ThisTransform != _hit.transform) continue;
                    chosenBuilding = building;
                    _collider = building.ObjectCollider;
                    _buildingParent = building.ParentTransform;
                    buildings.Remove(building);
                    currentAgent = null;
                    break;
                }
                foreach (var agent in agents)
                {
                    if (agent.transform != _hit.transform) continue;
                    currentAgent = agent;
                    return;
                }
                if (!currentAgent) return;
                currentAgent.destination = _hit.point;
            }
        }
    }

    private void DrawRaycasts()
    {
        if (OVRInput.Get(OVRInput.Touch.SecondaryIndexTrigger))
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, HandTransform.position);
            lineRenderer.SetPosition(1, HandTransform.forward + HandTransform.position);
        }
        else
        {
            lineRenderer.enabled = false;
        }

    }
}