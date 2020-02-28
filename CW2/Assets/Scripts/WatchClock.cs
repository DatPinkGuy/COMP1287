using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WatchClock : MonoBehaviour
{
    [SerializeField] private Text clockText;
    // Start is called before the first frame update
    void Start()
    {
        clockText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(UpdateTime());
    }

    IEnumerator UpdateTime()
    {
        var time = System.DateTime.Now;
        clockText.text = time.ToString("HH:mm");
        yield return new WaitForSeconds(0.2f); 
    }
}
