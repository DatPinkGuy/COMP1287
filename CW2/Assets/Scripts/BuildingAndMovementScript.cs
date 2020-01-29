using System;
using System.Collections;
using System.Collections.Generic;
using OVRTouchSample;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BuildingAndMovementScript : MonoBehaviour
{
    [Header("Current Agent/Object selected")]
    public BuildingInfo chosenBuilding;
    public NavMeshAgent currentAgent;
    private enum Cycle
    {
        Day,
        Night
    }
    private Cycle _cycle;
    private SunMoon _sunMoon;
    private RaycastHit _hit;
    private Ray _ray;
    private Collider _collider;
    private Transform _buildingParent;
    private int _currency;
    private int _layerMask = 1 << 8;
    private Transform HandTransform => rightHand.transform;
    private BuildingInfo PressedBuilding => _hit.transform.GetComponent<BuildingInfo>();
    private Vector3 HandRotation => leftHand.transform.rotation.eulerAngles;
    private bool _gameActive;
    private float _timer;
    private LaserPointer _laserPointer;
    [Header("Serialized Objects")]
    [SerializeField] private Hand rightHand;
    [SerializeField] private Hand leftHand;
    [SerializeField] private GameObject menuGameObject;
    //[SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private List<NavMeshAgent> agents;
    [SerializeField] private List<AgentCharacters> agentCharacter;
    [SerializeField] private List<BuildingInfo> buildings;
    [SerializeField] private Text currencyText;
    [SerializeField] private Text timerText;

    // Start is called before the first frame update
    void Start()
    {
        _gameActive = false;
        currencyText.text = _currency.ToString();
        _layerMask = ~_layerMask;
        _sunMoon = FindObjectOfType<SunMoon>();
        _laserPointer = FindObjectOfType<LaserPointer>();
        agents.AddRange(FindObjectsOfType<NavMeshAgent>());
        agentCharacter.AddRange(FindObjectsOfType<AgentCharacters>());
        buildings.AddRange(FindObjectsOfType<BuildingInfo>());
        _cycle = Cycle.Night;
    }
    
    // Update is called once per frame
    void Update()
    {
        //DrawRaycasts();
        OpenMenu();
        BuildingCharacterLogic();
        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            _gameActive = true;
            DayNightSwitch();
        }
        DayNightCycle();
        if (chosenBuilding)
        {
            MoveObjectToRaycast();
            PlaceObject();
        }
        if(_gameActive) UpdateTimer();
    }
    
    private void MoveObjectToRaycast()
    {
        _collider.enabled = false;
        if (Physics.Raycast(_ray, out _hit, 10, _layerMask))
        {
            _buildingParent.position = _hit.point;
        }
        else
        {
            _buildingParent.position = _laserPointer.MovingPosition;
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
            chosenBuilding.placed = true;
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
            if (Physics.Raycast(_ray, out _hit, 10, _layerMask))
            {
                foreach (var building in buildings)
                {
                    if (building.ThisTransform != _hit.transform) continue;
                    if (building.placed != false) continue;
                    chosenBuilding = building;
                    _collider = building.ObjectCollider;
                    _buildingParent = building.ParentTransform;
                    currentAgent = null;
                    chosenBuilding.MaterialChange();
                    break;

                }
                foreach (var agent in agents)
                {
                    if (agent.transform != _hit.transform) continue;
                    currentAgent = agent;
                    return;
                }
                if (!currentAgent) return;
                StartCoroutine(AgentMovement());
//                if (PressedBuilding) currentAgent.destination = PressedBuilding.ParentTransform.position;
//                else currentAgent.destination = _hit.point;
            }
        }
    }

//    private void DrawRaycasts()
//    {
//        if (OVRInput.Get(OVRInput.Touch.SecondaryIndexTrigger))
//        {
//            _ray = new Ray(HandTransform.position,HandTransform.forward);
//            Physics.Raycast(_ray, out _hit, 10, _layerMask);
//            lineRenderer.enabled = true;
//            lineRenderer.SetPosition(0, HandTransform.position);
//            if(_hit.collider)
//            {
//                lineRenderer.SetPosition(1, _hit.point);
//            }
//            else
//            {
//                lineRenderer.SetPosition(1, HandTransform.forward + HandTransform.position);
//            }
//            
//        }
//        else
//        {
//            lineRenderer.enabled = false;
//        }
//    }

    private void OpenMenu()
    {
        if (HandRotation.z > 140 && HandRotation.z < 210 && HandRotation.x > 0 && HandRotation.x < 40)
        {
            menuGameObject.SetActive(true);
        }
        else
        {
            menuGameObject.SetActive(false);
        }
    }

    private void UpdateCurrency()
    {
        currencyText.text = _currency.ToString();
    }

    private void UpdateTimer()
    {
        if (_gameActive)
        {
            _timer += Time.deltaTime;
            var timerRound = Math.Round(_timer, 2);
            timerText.text = timerRound.ToString();
        }
    }

    IEnumerator AgentMovement()
    {
        if (PressedBuilding)
        {
            currentAgent.destination = PressedBuilding.ParentTransform.position;
            if (PressedBuilding.Built)
            {
                yield return new WaitForSeconds(2);
//                if (currentAgent.transform.position == PressedBuilding.ParentTransform.position)
//                {
//                    currentAgent.transform.position = Vector3.Lerp(currentAgent.transform.position,
//                PressedBuilding.ChildTransform.position, 5f);
//                }
            }
        }
        else currentAgent.destination = _hit.point;
        yield return null;
    }
}