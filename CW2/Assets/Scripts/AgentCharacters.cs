using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]

public class AgentCharacters : MonoBehaviour, IAgent
{
    [Header("Agent Statistics")]
    public float health = 100;
    public float energy = 100;
    public float maxHealth = 100;
    public float maxEnergy = 100;
    public float agentSpeed;
    public Renderer meshRenderer;
    private BuildingAndMovementScript _mainScript;
    private NavMeshAgent Agent => GetComponent<NavMeshAgent>();
    private bool _walkingState;
    private bool _resetPath;
    private static readonly int AgentWalking = Animator.StringToHash("Walking");
    private static readonly int AgentBuilding = Animator.StringToHash("Building");
    private NavMeshAgent _navMeshAgent; 
    private Animator _agentAnimator;
    [HideInInspector] public Action changeAnimation;
    [HideInInspector] public float energyUsage = 10f;
    [HideInInspector] public float healthUsage = 1f;
    [HideInInspector] public bool buildingState = false;
    [SerializeField] private Image healthImage;
    [SerializeField] private Image energyImage;
    [SerializeField] private Canvas canvasBars;
    private float HealthBarValue
    {
        set => healthImage.fillAmount = value;
    }
    private float EnergyBarValue
    {
        set => energyImage.fillAmount = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        _mainScript = FindObjectOfType<BuildingAndMovementScript>();
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        _agentAnimator = GetComponent<Animator>();
        changeAnimation = IdleAnimation;

    }

    // Update is called once per frame
    void Update()
    {
        SetAnimations();
        CheckStats();
        UseHealth();
        CheckIfOnLink();
        RotateCanvas();
        ChangeHealthBar(health, maxHealth);
        ChangeEnergyBar(energy, maxEnergy);
    }

    public void UseEnergy()
    {
        energy -= energyUsage * Time.deltaTime;
    }

    public void UseHealth()
    {
        health -= healthUsage * Time.deltaTime;
    }

    public void CheckStats()
    {
        if (energy <= 0 || health <= 0)
        {
            gameObject.SetActive(false);
        }
        if (health > maxHealth) health = maxHealth;
        if (energy > maxEnergy) energy = maxEnergy;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.GetComponent<HealthPickUp>()) return;
        var pickUp = other.gameObject.GetComponent<HealthPickUp>();
        health += pickUp.healthAmount;
        pickUp.Destroy();
    }
 
    private void CheckIfOnLink()
    {
        if (Agent.isOnOffMeshLink) Agent.speed = agentSpeed/2;
        else Agent.speed = agentSpeed;
    }

    private void RotateCanvas()
    {
        Vector3 direction = _mainScript.centerCamera.transform.position - canvasBars.transform.position;
        canvasBars.transform.rotation = Quaternion.LookRotation(direction);
    }
    private void ChangeHealthBar(float value, float maxValue)
    {
        HealthBarValue = value / maxValue;
    }

    private void ChangeEnergyBar(float value, float maxValue)
    {
        EnergyBarValue = value / maxValue;
    }
    
    private void SetAnimations()
    {
        if (_mainScript.cycle == BuildingAndMovementScript.Cycle.Day)
        {
            if (Agent.hasPath) changeAnimation = MovingAnimation;
            else if (changeAnimation == BuildingAnimation) {}
            else changeAnimation = IdleAnimation;
            changeAnimation();
        }
        else
        {
            changeAnimation = IdleAnimation;
            changeAnimation();
        }
    }

    private void MovingAnimation()
    {
        buildingState = false;
        _walkingState = true;
        _agentAnimator.SetBool(AgentWalking,_walkingState);
        _agentAnimator.SetBool(AgentBuilding,buildingState);
        _resetPath = false;
    }

    private void IdleAnimation()
    {
        if (!_resetPath)
        {
            if(_mainScript.cycle == BuildingAndMovementScript.Cycle.Day) Agent.ResetPath();
            _resetPath = true;
        }
        _walkingState = false;
        buildingState = false;
        _agentAnimator.SetBool(AgentWalking,_walkingState);
        _agentAnimator.SetBool(AgentBuilding,buildingState);
    }

    public void BuildingAnimation()
    {
        buildingState = true;
        _walkingState = false;
        _agentAnimator.SetBool(AgentWalking,_walkingState);
        _agentAnimator.SetBool(AgentBuilding,buildingState);
    }

    public void ResetBuildingAnimation()
    {
        buildingState = false;
    }
}
