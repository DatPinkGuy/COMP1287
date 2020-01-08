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
    [SerializeField] private Camera cam;   
    [SerializeField] private List<NavMeshAgent> agents;
    [SerializeField] private List<BuildingInfo> buildings;

    // Start is called before the first frame update
    void Start()
    {
        agents.AddRange(FindObjectsOfType<NavMeshAgent>());
        buildings.AddRange(FindObjectsOfType<BuildingInfo>());
    }

    // Update is called once per frame
    void Update()
    {
        _ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(_ray, out _hit))
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
}