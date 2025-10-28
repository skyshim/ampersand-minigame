using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class to50_Timer : MonoBehaviour
{
    public TMP_Text timerText;
    public to50_GameManager gameManager;
    public float timer = 0f;
    private bool isTimerOn = true;

    // Update is called once per frame
    void Update()
    {
        if(isTimerOn && gameManager.isStarted)
        {
            timer += Time.deltaTime;
            timerText.text = timer.ToString("F2");
        }
    }

    public void StopTimer()
    {
        isTimerOn = false;
    }
}
