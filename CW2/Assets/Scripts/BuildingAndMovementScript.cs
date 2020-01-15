using System.Collections;
using System.Collections.Generic;
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
    private Cycle cycle;
    private SunMoon _sunMoon;
//    [SerializeField] private Camera cam;   
    [SerializeField] private List<NavMeshAgent> agents;
    [SerializeField] private List<AgentCharacters> agentCharacter;
    [SerializeField] private List<BuildingInfo> buildings;

    // Start is called before the first frame update
    void Start()
    {
        _sunMoon = FindObjectOfType<SunMoon>();
        agents.AddRange(FindObjectsOfType<NavMeshAgent>());
        agentCharacter.AddRange(FindObjectsOfType<AgentCharacters>());
        buildings.AddRange(FindObjectsOfType<BuildingInfo>());
        cycle = Cycle.Day;
    }
    
    // Update is called once per frame
    void Update()
    {
        BuildingCharacterLogic();
        if (OVRInput.GetDown(OVRInput.Button.Two)) DayNightSwitch();
        DayNightCycle();
        if (chosenBuilding)
        {
            MoveObjectToMouse();
            PlaceObject();
        }
    }
    
    private void MoveObjectToMouse()
    {
        _collider.enabled = false;
        if (!Physics.Raycast(_ray, out _hit)) return;
        _buildingParent.position = _hit.point;
        if (Input.GetKeyDown(KeyCode.E))
        {
            _buildingParent.transform.Rotate(0,15,0);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            _buildingParent.transform.Rotate(0,-15,0);
        }
    }

    private void PlaceObject()
    {
        if (chosenBuilding && Input.GetMouseButtonUp(0))
        {
            _collider.enabled = true;
            chosenBuilding = null;
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
//        _ray = cam.ScreenPointToRay(Input.mousePosition);
//        if (Input.GetMouseButtonDown(0))
//        {
//            if (Physics.Raycast(_ray, out _hit))
//            {
//                foreach (var building in buildings)
//                {
//                    if (building.ThisTransform != _hit.transform) continue;
//                    chosenBuilding = building;
//                    _collider = building.ObjectCollider;
//                    _buildingParent = building.ParentTransform;
//                    buildings.Remove(building);
//                    currentAgent = null;
//                    break;
//                }
//                foreach (var agent in agents)
//                {
//                    if (agent.transform != _hit.transform) continue;
//                    currentAgent = agent;
//                    return;
//                }
//                if (!currentAgent) return;
//                currentAgent.destination = _hit.point;
//            } 
//        }
       
    }
}