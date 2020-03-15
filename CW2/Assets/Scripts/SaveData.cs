using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class SaveData
{
    public int level;
    public int currencyAmountRestart;
    public int currencyAmount;
    public bool[] boughtRestart;
    public bool[] bought;
    public bool snapSetting;
    public float speedSetting;
    public float rotateSetting;

    public SaveData(Watch watch)
    {
        currencyAmount = watch.currency;
        currencyAmountRestart = watch.LevelStartCurrency;
    }

    public SaveData(UpgradeMain upgradeMain)
    {
        
    }

    public SaveData(SettingsMain settings)
    {
        snapSetting = settings.SnapSettingsPublic;
        speedSetting = settings.SpeedSettingsPublic;
        rotateSetting = settings.RotationSettingsPublic;
    }

    public SaveData(SceneChanging sceneChanging)
    {
        level = sceneChanging.CurrentLevel;
    }
}
