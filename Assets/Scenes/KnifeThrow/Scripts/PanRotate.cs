using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanRotate : MonoBehaviour
{

    public float baseSpeed = 100f;

    private float currentSpeed;
    private int pattern = 1;
    private float time;
    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = baseSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        switch(pattern)
        {
            case 1: //기본
                currentSpeed = baseSpeed;
                break;
            case 2: //약간빠름
                currentSpeed = baseSpeed * 1.3f;
                break;
            case 3: //약간느림
                currentSpeed = baseSpeed * 0.8f;
                break;
            case 4: //진동
                currentSpeed = baseSpeed * Mathf.Sin(time * 0.5f) * 1.5f;
                break;
        }

        transform.Rotate(0, 0, currentSpeed * Time.deltaTime);
    }

    public void SetPattern(int patternSet)
    {
        pattern = patternSet;
        time = 0;
    }
}
