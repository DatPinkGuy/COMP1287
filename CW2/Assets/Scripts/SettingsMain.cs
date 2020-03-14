using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMain : MonoBehaviour
{
    protected static bool SnapSettings;
    protected static float SpeedSettings = 0.6f;
    protected static float RotationSettings = 1f;
    private static readonly object Padlock = new object();
    private static OVRPlayerController _ovrPlayerController;
    protected static OVRPlayerController OvrPlayerController
    {
        get
        {
            lock (Padlock)
            {
                if (_ovrPlayerController == null)
                {
                    _ovrPlayerController = FindObjectOfType<OVRPlayerController>();
                }
                return _ovrPlayerController;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        OvrPlayerController.SnapRotation = SnapSettings;
        OvrPlayerController.Acceleration = SpeedSettings;
        OvrPlayerController.RotationAmount = RotationSettings;
    }
}
