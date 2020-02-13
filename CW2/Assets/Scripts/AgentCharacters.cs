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
    [HideInInspector] public float energyUsage = 10f;
    [HideInInspector] public float healthUsage = 1f;
    [SerializeField] private Image healthImage;
    [SerializeField] private Image energyImage;
    [SerializeField] private Camera cameraToFollow;
    [SerializeField] private Canvas canvasBars;
    private NavMeshAgent Agent => GetComponent<NavMeshAgent>();
    private float HealthBarValue
    {
        get => healthImage.fillAmount;
        set => healthImage.fillAmount = value;
    }

    private float EnergyBarValue
    {
        get => energyImage.fillAmount;
        set => energyImage.fillAmount = value;
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
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
        Vector3 direction = cameraToFollow.transform.position - canvasBars.transform.position;
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
}
