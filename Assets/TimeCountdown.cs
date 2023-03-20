using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeCountdown : MonoBehaviour
{
    public float timeAmount;//time in seconds
    public Text timeText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeAmount += Time.deltaTime;//that the time counts up

        ShowTime(timeAmount);
    }
    void ShowTime(float timeToShow)
    {
        float minutes = Mathf.FloorToInt(timeToShow / 60) -1;//round the time - get it in seconds - minus 1 because the Timer startet with 01:00
        float seconds = Mathf.FloorToInt(timeToShow % 60);//display seconds that are left

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);//right of the dots is the format
    }
}
