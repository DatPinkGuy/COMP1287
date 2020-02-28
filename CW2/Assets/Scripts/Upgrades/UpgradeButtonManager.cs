using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButtonManager : MonoBehaviour
{
    public Text textField;
    protected AudioSource ButtonClick => GetComponent<AudioSource>();
    [HideInInspector] public List<IndexFinger> indexFinger = new List<IndexFinger>();
    [HideInInspector] public List<Collider> fingerColliders = new List<Collider>();
    // Start is called before the first frame update
    void Start()
    {
        textField.text = null;
        indexFinger.AddRange(FindObjectsOfType<IndexFinger>());
        foreach (var finger in indexFinger)
        {
            fingerColliders.Add(finger.GetComponent<Collider>());
        }
    }

    public virtual void ButtonPress()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        foreach (var finger in fingerColliders)
        {
            if (other != finger) continue;
            if (ButtonClick) ButtonClick.Play();
            textField.text = null;
        }
    }
}
