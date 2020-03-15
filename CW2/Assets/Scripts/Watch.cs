using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OVRTouchSample;

public class Watch : MonoBehaviour
{
    [HideInInspector] public int woodCount;
    [HideInInspector] public int currency;
    public int LevelStartCurrency { get; set; }
    public float Timer { get; private set; }
    private Vector3 HandRotation => leftHand.transform.rotation.eulerAngles;
    private string Minutes => Mathf.Floor(Timer / 60).ToString("00");
    private string Seconds => Mathf.Floor(Timer % 60).ToString("00");
    private BuildingAndMovementScript _mainScript;
    [SerializeField] private GameObject menuGameObject;
    [SerializeField] private GameObject bigMenuGameObject;
    [SerializeField] private Hand leftHand;
    [SerializeField] private Text currencyText;
    [SerializeField] private Text timerText;
    [SerializeField] private Text woodText;
    // Start is called before the first frame update
    void Start()
    {
        var data = SaveSystem.LoadCurrency();
        if (data != null)
        {
            LevelStartCurrency = data.currencyAmountRestart;
            currency = data.currencyAmount;
        }
        if (LevelStartCurrency > currency) currency = LevelStartCurrency;
        else LevelStartCurrency = currency;
        SaveSystem.SaveCurrency(this);
        currencyText.text = currency.ToString();
        _mainScript = FindObjectOfType<BuildingAndMovementScript>();
        UpdateCurrency();
    }

    // Update is called once per frame
    void Update()
    {
        OpenMenu();
        if(_mainScript.GameActive) UpdateTimer();
    }
    
    private void OpenMenu()
    {
        if (HandRotation.z > 140 && HandRotation.z < 210 && HandRotation.x > 0 && HandRotation.x < 40 && !bigMenuGameObject.activeSelf)
        {
            menuGameObject.SetActive(true);
        }
        else
        {
            menuGameObject.SetActive(false);
        }
    }

    public void UpdateCurrency()
    {
        currencyText.text = currency.ToString();
    }

    public void UpdateWood()
    {
        woodText.text = woodCount.ToString();
    }
    
    private void UpdateTimer()
    {
        Timer += Time.deltaTime;
        timerText.text = Minutes + ":" + Seconds;
    }
}
