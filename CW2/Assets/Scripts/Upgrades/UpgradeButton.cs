using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeButton : UpgradeButtonManager
{
    [SerializeField] private int buttonNumber;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other == collider) ButtonPress();
    }

    public override void ButtonPress()
    {
        if (textField.text.Length == 2) return;
        var currentText = textField.text;
        textField.text = null;
        textField.text = currentText + buttonNumber.ToString();
    }
}
