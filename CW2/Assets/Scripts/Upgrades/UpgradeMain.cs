using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMain : MonoBehaviour
{
    public int upgradeNumber;
    public Material[] materials;
    public BuildingAndMovementScript mainScript;

    private void Start()
    {
        mainScript = FindObjectOfType<BuildingAndMovementScript>();
    }

    public virtual void UseUpgrade()
    {
        
    }
}
