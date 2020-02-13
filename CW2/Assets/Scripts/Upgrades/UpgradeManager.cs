using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private UpgradeButtonManager buttonManager;
    [SerializeField] private Collider handCollider;
    [SerializeField] private UpgradeMain[] listOfUpgrades;
    // Start is called before the first frame update
    void Start()
    {
        listOfUpgrades = FindObjectsOfType<UpgradeMain>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if(other != handCollider) return;
        foreach (var upgrade in listOfUpgrades)
        {
            if (upgrade.upgradeNumber == 0) return;
            if (upgrade.upgradeNumber == int.Parse(buttonManager.textField.text))
            {
                buttonManager.textField.text = null;
                upgrade.UseUpgrade();
                break;
            }
        }
    }
}
