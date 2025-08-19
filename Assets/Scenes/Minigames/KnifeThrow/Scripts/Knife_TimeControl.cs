using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Knife_TimeControl : MonoBehaviour
{
    public Image timerBar;
    public float maxTime = 15f;
    public float currentTime;

    private bool isRunning = false;
    private Knife_RoundManager roundManager;

    void Start()
    {
        roundManager = GameObject.Find("GameController").GetComponent<Knife_RoundManager>();
        StartTimer();
    }

    void Update()
    {
        if (!isRunning) return;

        currentTime -= Time.deltaTime;
        timerBar.fillAmount = currentTime / maxTime;

        if (currentTime <= 0)
        {
            isRunning = false;
            roundManager.OnTimeOver();
        }
    }

    public void StartTimer()
    {
        currentTime = maxTime;
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }
}
