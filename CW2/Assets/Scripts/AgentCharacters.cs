using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]

public class AgentCharacters : MonoBehaviour, IAgent
{
    public float health = 100;
    public float maxHealth = 100;
    public float energy = 100;
    public float maxEnergy = 100;
    [HideInInspector] public float energyUsage = 10f;
    [HideInInspector] public float healthUsage = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UseHealth();
        CheckStats();
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
            Destroy(this.gameObject);
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
}
