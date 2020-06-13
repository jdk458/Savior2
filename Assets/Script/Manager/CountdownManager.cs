using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownManager : MonoBehaviour
{
    public float time = 600;
    float tempTime = 0;
    [HideInInspector] public float remainTime;

    private void FixedUpdate()
    {
        if (TimeManager.instance.GetTime())
            return;

        tempTime += Time.fixedDeltaTime;

        remainTime = time - tempTime;
        string minuteString = ((int)remainTime / 60).ToString("00");
        string secondString = ((int)remainTime % 60).ToString("00");
        this.GetComponent<Text>().text = minuteString + " : " + secondString;
        if (tempTime >= time)
        {
            tempTime = 0;
            StageManager.instance.NextStage();
        }
    }
}
