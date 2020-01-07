using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]

public class AiCharacter : MonoBehaviour, IInterface
{
    public float health = 100;
    public float energy = 100;
    [SerializeField] private float energyUsage = 10f;
    [SerializeField] private float healthUsage = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //UseEnergy();
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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.GetComponent<HealthPickUp>()) return;
        var pickUp = other.gameObject.GetComponent<HealthPickUp>();
        health += pickUp.healthAmount;
        pickUp.Destroy();
    }
}
