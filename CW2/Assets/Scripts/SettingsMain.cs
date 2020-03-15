using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMain : MonoBehaviour
{
    protected static bool SettingsLoaded;
    protected static bool SnapSettings;
    protected static float SpeedSettings = 0.6f;
    protected static float RotationSettings = 1f;
    public bool SnapSettingsPublic => SnapSettings;
    public float SpeedSettingsPublic => SpeedSettings;
    public float RotationSettingsPublic => RotationSettings;
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
        if (!SettingsLoaded) LoadSettings();
        OvrPlayerController.SnapRotation = SnapSettings;
        OvrPlayerController.Acceleration = SpeedSettings;
        OvrPlayerController.RotationAmount = RotationSettings;
    }

    public void SaveSettings()
    {
        SaveSystem.SaveSettings(this);
    }

    public void LoadSettings()
    {
        var data = SaveSystem.LoadSettings();
        if (data == null) return;
        SnapSettings = data.snapSetting;
        SpeedSettings = data.speedSetting;
        RotationSettings = data.rotateSetting;
        SettingsLoaded = true;
    }

    public void NewGame()
    {
        SaveSystem.ClearSaveData();
    }
}
