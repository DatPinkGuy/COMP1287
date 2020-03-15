using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMain : MonoBehaviour
{
    public int upgradeNumber;
    public Material[] materials;
    public Watch watchScript;

    private void Start()
    {
        watchScript = FindObjectOfType<Watch>();
    }

    public virtual void CheckUpgrade()
    {
        
    }

    public virtual void ApplyUpgrade()
    {
        
    }
}
