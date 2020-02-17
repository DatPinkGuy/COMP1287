using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    private  List<IndexFinger> _indexFinger = new List<IndexFinger>();
    private List<Collider> _fingerColliders = new List<Collider>();
    [SerializeField] private UpgradeButtonManager buttonManager;
    [SerializeField] private UpgradeMain[] listOfUpgrades;
    // Start is called before the first frame update
    void Start()
    {
        listOfUpgrades = FindObjectsOfType<UpgradeMain>();
        _indexFinger.AddRange(FindObjectsOfType<IndexFinger>());
        foreach (var finger in _indexFinger)
        {
            _fingerColliders.Add(finger.GetComponent<Collider>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    private void OnTriggerEnter(Collider other)
    {
        foreach (var finger in _fingerColliders)
        {
            if (other != finger) continue;
            foreach (var upgrade in listOfUpgrades)
            {
                if (upgrade.upgradeNumber == 0) return;
                if (upgrade.upgradeNumber == int.Parse(buttonManager.textField.text ?? throw new IndexOutOfRangeException()))
                {
                    buttonManager.textField.text = null;
                    upgrade.UseUpgrade();
                    break;
                }
            }
        }
    }
}
