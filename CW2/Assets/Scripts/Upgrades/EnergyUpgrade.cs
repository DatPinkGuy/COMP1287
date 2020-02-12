using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyUpgrade : UpgradeMain
{
    
    private bool _enoughCurrency;
    private bool _bought;
    private MeshRenderer ObjectMaterial => GetComponent<MeshRenderer>();
    [SerializeField] private int price;
    [SerializeField] private int energyIncrease;
    [SerializeField] private AgentCharacters[] agents;

    // Start is called before the first frame update
    void Start()
    {
        agents = FindObjectsOfType<AgentCharacters>();
        if (!_bought) ObjectMaterial.material = materials[0];
        else ObjectMaterial.material = materials[1];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void UseUpgrade()
    {
        if (_bought) return;
        if (price < mainScript.currency) return;
        foreach (var agent in agents)
        {
            agent.maxEnergy += energyIncrease;
            agent.energy += energyIncrease;
        }
        _bought = true;
        ObjectMaterial.material = materials[1];
    }
}
