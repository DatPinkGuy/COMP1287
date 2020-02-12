using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButtonManager : MonoBehaviour
{
    public Text textField;
    // Start is called before the first frame update
    void Start()
    {
        textField.text = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void ButtonPress()
    {
        
    }
}
