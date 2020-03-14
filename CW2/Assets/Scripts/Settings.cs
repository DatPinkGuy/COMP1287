using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : SettingsMain
{
    [SerializeField] private Toggle snapToggle;
    [SerializeField] private Slider speedSlider;
    [SerializeField] private GameObject rotationUi;
    [SerializeField] private Slider rotationSlider;
    // Start is called before the first frame update
    void Start()
    {
        SnapRotation(SnapSettings);
        snapToggle.isOn = SnapSettings;
        speedSlider.value = SpeedSettings;
        rotationSlider.value = RotationSettings;
    }

    public void SnapRotation(bool isSnapped)
    {
        OvrPlayerController.SnapRotation = isSnapped;
        SnapSettings = isSnapped;
        rotationUi.SetActive(!isSnapped);
    }

    public void SetSpeed(float speed)
    {
        OvrPlayerController.Acceleration = speed;
        SpeedSettings = speed;
    }
    
    public void SetRotation(float amount)
    {
        OvrPlayerController.RotationAmount = amount;
        RotationSettings = amount;
    }
}
