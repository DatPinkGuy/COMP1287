using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    
}
