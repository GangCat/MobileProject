using DI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextGold : DIMono
{
    [Inject]
    UserData userData;

    public TMP_Text goldText;

    int prevGold;
    int prevVisualGold;
    int visualGold;

    public override void Init()
    {
        prevVisualGold = userData.gold;
        visualGold = prevVisualGold;
        prevGold = userData.gold;
        goldText.text = prevVisualGold.ToString();
    }

    float changeStartTime;
    float completeChangeTime;
    public float changeDuration = 0.5f;
    public int changeStartValue;


    private void Update()
    {
        if (prevGold != userData.gold)
        {
            prevGold = userData.gold;
            changeStartValue = visualGold;
            changeStartTime = Time.time;
            completeChangeTime = changeStartTime + changeDuration;
        }

        if (Time.time < completeChangeTime)
        {
            var elapse = Time.time - changeStartTime;
            var t= elapse / changeDuration;

            visualGold = (int)Mathf.Lerp(changeStartValue, userData.gold, t);
        }
        else
        {
            visualGold = userData.gold;
        }
        
        if(visualGold!= prevVisualGold)
        {
            prevVisualGold = visualGold;
            goldText.text = prevVisualGold.ToString();
        }
    }
}
