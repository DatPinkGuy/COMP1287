using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUpgrade : UpgradeMain
{
    private static bool _bought;
    private MeshRenderer ObjectMaterial => GetComponent<MeshRenderer>();
    [SerializeField] private int price;
    [SerializeField] private int healthIncrease;
    [SerializeField] private List<AgentCharacters> agents;

    // Start is called before the first frame update
    void Start()
    {
        watchScript = FindObjectOfType<Watch>();
        agents.AddRange(FindObjectsOfType<AgentCharacters>());
        ObjectMaterial.material = !_bought ? materials[0] : materials[1];
    }
    
    public override void UseUpgrade()
    {
        if (_bought) return;
        if (watchScript.currency < price) return;
        foreach (var agent in agents)
        {
            agent.maxHealth += healthIncrease;
            agent.health += healthIncrease;
        }
        watchScript.currency -= price;
        _bought = true;
        watchScript.UpdateCurrency();
        ObjectMaterial.material = materials[1];
    }
}
