using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class PressedButton : Button
{
    public UnityEvent whilePressed;

    public float pressInterval = 0.1f;

    public float pressButtonDelay = 1f;

    bool isPressed = false;
    int lastNum;
    float startTime;
    // Update is called once per frame
    void Update()
    {
        
        if (IsPressed())
        {
            if (isPressed == false)
            {
                startTime = Time.time;
            }
            isPressed = true;
            if (Time.time - startTime < pressButtonDelay)
                return;

            var num =Mathf.FloorToInt( Time.time / pressInterval);
            if (num != lastNum)
            {
                whilePressed.Invoke();
                lastNum = num;
            }
        }
        else
        {
            isPressed = false;
        }

    }
}
