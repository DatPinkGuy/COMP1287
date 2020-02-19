using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUpgrade : UpgradeMain
{
    private bool _enoughCurrency;
    private bool _bought;
    private MeshRenderer ObjectMaterial => GetComponent<MeshRenderer>();
    [SerializeField] private int price;
    [SerializeField] private int healthIncrease;
    [SerializeField] private List<AgentCharacters> agents;

    // Start is called before the first frame update
    void Start()
    {
        mainScript = FindObjectOfType<BuildingAndMovementScript>();
        agents.AddRange(FindObjectsOfType<AgentCharacters>());
        ObjectMaterial.material = !_bought ? materials[0] : materials[1];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void UseUpgrade()
    {
        if (_bought) return;
        if (mainScript.currency < price) return;
        foreach (var agent in agents)
        {
            agent.maxHealth += healthIncrease;
            agent.health += healthIncrease;
        }
        _bought = true;
        ObjectMaterial.material = materials[1];
    }
}
