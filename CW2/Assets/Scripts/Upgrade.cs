using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Upgrade : MonoBehaviour
{
    protected bool UpgradeBought = false;
    [SerializeField]protected int upgradeNumber;
    protected BuildingAndMovementScript MainScript;

    private void Awake()
    {
        MainScript = FindObjectOfType<BuildingAndMovementScript>();
        CheckUpgrade();
    }

    public abstract void CheckUpgrade();
    public abstract void UseUpgrade();
}
