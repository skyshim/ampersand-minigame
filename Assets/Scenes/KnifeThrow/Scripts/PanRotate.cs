using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RotatePattern
{
    Basic = 1,
    Fast = 2,
    Slow = 3,
    Osilate = 4,
    Blink = 5,
    SizeOsilate = 6,
    Superfast = 7,
    Orbit = 8,
}
public class PanRotate : MonoBehaviour
{

    public float baseSpeed = 100f;
    public Transform orbitPivot;

    private float currentSpeed;
    private RotatePattern pattern;
    private float time;
    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = baseSpeed;
        orbitPivot = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        switch (pattern)
        {
            case RotatePattern.Basic:
                currentSpeed = baseSpeed;
                break;
            case RotatePattern.Fast:
                currentSpeed = baseSpeed * 1.3f;
                break;
            case RotatePattern.Slow:
                currentSpeed = baseSpeed * 0.8f;
                break;
            case RotatePattern.Osilate:
                currentSpeed = baseSpeed * Mathf.Sin(time * 0.8f) * 2f;
                break;
            case RotatePattern.Blink:
                currentSpeed = baseSpeed * 1.1f;

                bool visible = ((int)time % 2 == 0); // 홀수 초에 보이게

                foreach (Renderer r in GetComponentsInChildren<Renderer>())
                {
                    r.enabled = visible;
                }
                break;
            case RotatePattern.SizeOsilate:
                currentSpeed = baseSpeed * 1.1f;
                float scale = 2.5f + Mathf.Sin(time)*0.5f;
                transform.localScale = new Vector3(scale, scale, 1f);
                break;
            case RotatePattern.Superfast:
                currentSpeed = baseSpeed * 2f;
                break;
            case RotatePattern.Orbit:
                currentSpeed = baseSpeed * 0.5f;
                orbitPivot.Rotate(0, 0, currentSpeed * Time.deltaTime*1.5f);
                break;
        }

        transform.Rotate(0, 0, currentSpeed * Time.deltaTime);
    }

    public void SetPattern(RotatePattern patternSet)
    {
        pattern = patternSet;
        if (pattern == RotatePattern.Orbit) transform.position = new Vector3(1, 0, 0);
        time = 0;
    }
}
