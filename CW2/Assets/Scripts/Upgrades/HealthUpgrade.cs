using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUpgrade : UpgradeMain
{
    private MeshRenderer ObjectMaterial => GetComponent<MeshRenderer>();
    [SerializeField] private int price;
    [SerializeField] private int healthIncrease;
    [SerializeField] private List<AgentCharacters> agents;

    // Start is called before the first frame update
    void Start()
    {
        FindClasses();
        agents.AddRange(FindObjectsOfType<AgentCharacters>());
        boughtRestart = bought;
        if(bought) ApplyUpgrade();
    }

    public override void CheckUpgrade()
    {
        if (bought) return;
        if (watchScript.currency < price) return;
        ApplyUpgrade();
    }

    public override void ApplyUpgrade()
    {
        foreach (var agent in agents)
        {
            agent.maxHealth += healthIncrease;
            agent.health += healthIncrease;
        }
        ObjectMaterial.material = materials[1];
        if (bought) return;
        watchScript.currency -= price;
        bought = true;
        watchScript.UpdateCurrency();
    }
}
