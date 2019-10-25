using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingScript : MonoBehaviour
{
    private RaycastHit _hit;
    private Ray _ray;
    private Collider _collider;
    private BuildingCollider ColliderBuilding => chosenBuilding.GetComponent<BuildingCollider>();
    public GameObject chosenBuilding;
    [SerializeField] private new Camera cam;
    [SerializeField] private List<GameObject> buildings;

    // Start is called before the first frame update
    void Start()
    {

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
                    if (building.transform == _hit.transform)
                    {
                        chosenBuilding = building;
                        _collider = ColliderBuilding.ObjectCollider;
                        buildings.Remove(building);
                        break;
                    }
                }
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
        chosenBuilding.transform.position = _hit.point;
        chosenBuilding.transform.rotation = Quaternion.FromToRotation(Vector3.up, _hit.normal);
    }

    private void PlaceObject()
    {
        if (chosenBuilding && Input.GetMouseButtonUp(0))
        {
            _collider.enabled = true;
            chosenBuilding = null;
        }
    }
}