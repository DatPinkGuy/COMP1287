using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyUpgrade : Upgrade
{
    [SerializeField] private float increaseAmount;
    // Start is called before the first frame update
    void Start()
    {
        //CheckUpgrade();
    }

    public override void CheckUpgrade()
    {
        if (UpgradeBought) this.enabled = false;
    }

    public override void UseUpgrade()
    {
        if(UpgradeBought) return;
        foreach (var agent in MainScript.PublicAgents)
        {
            agent.maxEnergy += increaseAmount;
        }
        UpgradeBought = true;
    }
}
