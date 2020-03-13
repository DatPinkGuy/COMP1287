using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    private OVRPlayerController _ovrPlayerController;
    // Start is called before the first frame update
    void Start()
    {
        _ovrPlayerController = FindObjectOfType<OVRPlayerController>();
        SnapRotation(_ovrPlayerController.SnapRotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SnapRotation(bool isSnapped)
    {
        _ovrPlayerController.SnapRotation = isSnapped;
    }
}
