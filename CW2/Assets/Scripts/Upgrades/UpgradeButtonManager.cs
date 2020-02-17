using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButtonManager : MonoBehaviour
{
    public Text textField;
    [HideInInspector] public List<IndexFinger> _indexFinger = new List<IndexFinger>();
    [HideInInspector] public List<Collider> _fingerColliders = new List<Collider>();
    // Start is called before the first frame update
    void Start()
    {
        textField.text = null;
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

    public virtual void ButtonPress()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        foreach (var finger in _fingerColliders)
        {
            if (other != finger) continue;
            textField.text = null;
        }
    }
}
