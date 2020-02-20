﻿using System;
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
    [HideInInspector] public int woodCount;
    [HideInInspector] public float timer;
    [HideInInspector] public bool gameActive;
    [HideInInspector] public int currency;
    private BuildingInfo _chosenBuilding;
    private NavMeshAgent _currentAgent;
    private SunMoon _sunMoon;
    private RaycastHit _hit;
    private Ray _ray;
    private Transform _buildingParent;
    private int _layerMask = 1 << 8;
    private Transform HandTransform => rightHand.transform;
    private Vector3 HandRotation => leftHand.transform.rotation.eulerAngles;
    private LaserPointer _laserPointer;
    private int _agentIndex = 0;
    private Renderer AgentMaterial => _currentAgent.GetComponent<Renderer>();
    private string Minutes => Mathf.Floor(timer / 60).ToString("00");
    private string Seconds => Mathf.Floor(timer % 60).ToString("00");

    [Header("Serialized Objects")]
    [SerializeField] private Hand rightHand;
    [SerializeField] private Hand leftHand;
    [SerializeField] private GameObject menuGameObject;
    [SerializeField] private List<NavMeshAgent> agents;
    [SerializeField] private List<AgentCharacters> agentCharacter;
    [SerializeField] private List<BuildingInfo> buildings;
    [SerializeField] private Text currencyText;
    [SerializeField] private Text timerText;
    [SerializeField] private Text woodText;

    // Start is called before the first frame update
    void Start()
    {
        gameActive = false;
        currencyText.text = currency.ToString();
        _layerMask = ~_layerMask;
        _sunMoon = FindObjectOfType<SunMoon>();
        _laserPointer = FindObjectOfType<LaserPointer>();
        agents.AddRange(FindObjectsOfType<NavMeshAgent>());
        agentCharacter.AddRange(FindObjectsOfType<AgentCharacters>());
        buildings.AddRange(FindObjectsOfType<BuildingInfo>());
        cycle = Cycle.Night;
    }
    
    // Update is called once per frame
    void Update()
    {
        OpenMenu();
        if (OVRInput.GetDown(OVRInput.Button.Four)) ChangeCharacter();
        BuildingCharacterLogic();
        if (OVRInput.GetDown(OVRInput.Button.Three))
        {
            gameActive = true;
            DayNightSwitch();
        }
        DayNightCycle();
        if (_chosenBuilding)
        {
            MoveObjectToRaycast();
            PlaceObject();
        }
        UpdateCurrency();
        UpdateWood();
        if(gameActive) UpdateTimer();
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
                    if (building.Built && _currentAgent) continue;
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
        if(_currentAgent) AgentMaterial.material.DisableKeyword("_EMISSION");
        _agentIndex++;
        if (_agentIndex <= agents.Count)
        {
            _currentAgent = agents[_agentIndex-1];
            AgentMaterial.material.EnableKeyword("_EMISSION");
        }
        else if (_agentIndex == agents.Count+1) _currentAgent = null;
        else
        {
            _agentIndex = 1;
            _currentAgent = agents[_agentIndex-1];
            AgentMaterial.material.EnableKeyword("_EMISSION");
        }
        
    }
    
    private void MoveObjectToRaycast()
    {
        _chosenBuilding.ObjectCollider.enabled = false;
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
                foreach (var agent in agentCharacter)
                {
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
        currencyText.text = currency.ToString();
    }

    private void UpdateTimer()
    {
        if (gameActive)
        {
            timer += Time.deltaTime;
            timerText.text = Minutes + ":" + Seconds;
        }
    }

    private void UpdateWood()
    {
        woodText.text = woodCount.ToString();
    }
    
    IEnumerator AgentMovement()
    {
        _currentAgent.destination = _hit.point;
        yield return null;
    }
}