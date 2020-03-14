using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OVRTouchSample;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BuildingAndMovementScript : MonoBehaviour
{
    public enum Cycle { Day, Night }
    public Cycle cycle;
    public Camera centerCamera;
    private BuildingInfo _chosenBuilding;
    private NavMeshAgent _currentAgent;
    private SunMoon _sunMoon;
    private RaycastHit _hit;
    private NavMeshHit _navMeshHit;
    private Ray _ray;
    private Transform _buildingParent;
    private int _layerMask = 1 << 8;
    private Transform HandTransform => rightHand.transform;
    private LaserPointer _laserPointer;
    private int _agentIndex;
    private EndZone _endZone;
    private AgentCharacters CurrentAgentScript => _currentAgent.GetComponent<AgentCharacters>();
    public bool GameActive { get; set; }
    [Header("Serialized Objects")]
    [SerializeField] private Hand rightHand;
    [SerializeField] private List<NavMeshAgent> agents;
    [SerializeField] private List<BuildingInfo> buildings;

    // Start is called before the first frame update
    void Start()
    {
        GameActive = false;
        _layerMask = ~_layerMask;
        _sunMoon = FindObjectOfType<SunMoon>();
        _laserPointer = FindObjectOfType<LaserPointer>();
        _endZone = FindObjectOfType<EndZone>();
        agents.AddRange(FindObjectsOfType<NavMeshAgent>());
        buildings.AddRange(FindObjectsOfType<BuildingInfo>());
        cycle = Cycle.Night;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (_endZone.GameEnd) return;
        if (OVRInput.GetDown(OVRInput.Button.Four)) ChangeCharacter();
        BuildingCharacterLogic();
        if (OVRInput.GetDown(OVRInput.Button.Three))
        {
            GameActive = true;
            DayNightSwitch();
        }
        DayNightCycle();
        if (_chosenBuilding)
        {
            MoveObjectToRaycast();
            PlaceObject();
        }
    }
    
    private void BuildingCharacterLogic()
    {
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            _ray = new Ray(HandTransform.position,HandTransform.forward);
            if (Physics.Raycast(_ray, out _hit, 10, _layerMask))
            {
                foreach (var building in buildings)
                {
                    if (building.ThisTransform != _hit.transform) continue;
                    if (building.Built) return;
                    if (_currentAgent)
                    {
                        StartCoroutine(AgentMovement());
                        return;
                    }
                    _chosenBuilding = building;
                    _buildingParent = building.ParentTransform;
                    _currentAgent = null;
                    _chosenBuilding.MaterialChange();
                    break;

                }
                if (!_currentAgent) return;
                StartCoroutine(AgentMovement());
            }
        }
    }

    private void ChangeCharacter()
    {
        if(_currentAgent) CurrentAgentScript.MeshRenderer.material.DisableKeyword("_EMISSION");
        _agentIndex++;
        if (_agentIndex <= agents.Count)
        {
            _currentAgent = agents[_agentIndex-1];
            CurrentAgentScript.MeshRenderer.material.EnableKeyword("_EMISSION");
        }
        else if (_agentIndex == agents.Count+1) _currentAgent = null;
        else
        {
            _agentIndex = 1;
            _currentAgent = agents[_agentIndex-1];
            CurrentAgentScript.MeshRenderer.material.EnableKeyword("_EMISSION");
        }
        
    }
    
    private void MoveObjectToRaycast()
    {
        _chosenBuilding.ObjectCollider.enabled = false;
        _ray = new Ray(HandTransform.position,HandTransform.forward);
        if (Physics.Raycast(_ray, out _hit, 10, _layerMask))
        {
            _buildingParent.position = _hit.point;
        }
        else
        {
            _buildingParent.position = _laserPointer.MovingPosition;
        }
        if (OVRInput.GetDown(OVRInput.Button.Two)) 
        {
            _buildingParent.transform.Rotate(0,15,0);
        }
        else if (OVRInput.GetDown(OVRInput.Button.One)) 
        { 
            _buildingParent.transform.Rotate(0, -15, 0);
        }

    }

    private void PlaceObject()
    {
        if (_chosenBuilding && OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger))
        {
            _chosenBuilding.ObjectCollider.enabled = true;
            _chosenBuilding = null;
            _buildingParent = null;
        }
    }

    private void DayNightSwitch()
    {
        cycle = cycle == Cycle.Day ? Cycle.Night : Cycle.Day;
    }

    private void DayNightCycle()
    {
        switch (cycle)
        {
            case Cycle.Day:
                foreach (var agent in agents)
                {
                    agent.enabled = true;
                }
                _sunMoon.ChangeToSun();
                break;
            case Cycle.Night:
                foreach (var agent in agents)
                {
                    agent.enabled = false;
                }
                _sunMoon.ChangeToMoon();
                break;
        }
    }

    IEnumerator AgentMovement()
    {
        if (NavMesh.SamplePosition(_hit.point, out _navMeshHit, 50, NavMesh.AllAreas))
        {
            _currentAgent.destination = _navMeshHit.position;
        }
        yield return null;
    }
}