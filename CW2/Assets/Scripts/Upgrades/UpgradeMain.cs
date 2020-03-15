using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMain : MonoBehaviour
{
    public bool boughtRestart;
    public bool bought;
    public int upgradeNumber;
    public Material[] materials;
    public Watch watchScript;

    public virtual void CheckUpgrade()
    {
        
    }

    public virtual void ApplyUpgrade()
    {
        
    }

    protected void FindClasses()
    {
        watchScript = FindObjectOfType<Watch>();
    }
}
