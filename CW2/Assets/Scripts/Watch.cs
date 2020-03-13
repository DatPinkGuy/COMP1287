using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OVRTouchSample;

public class Watch : MonoBehaviour
{
    [HideInInspector] public int woodCount;
    [HideInInspector] public float timer;
    [HideInInspector] public int currency;
    private Vector3 HandRotation => leftHand.transform.rotation.eulerAngles;
    private string Minutes => Mathf.Floor(timer / 60).ToString("00");
    private string Seconds => Mathf.Floor(timer % 60).ToString("00");
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
        currencyText.text = currency.ToString();
        _mainScript = FindObjectOfType<BuildingAndMovementScript>();
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
        timer += Time.deltaTime;
        timerText.text = Minutes + ":" + Seconds;
    }
}
