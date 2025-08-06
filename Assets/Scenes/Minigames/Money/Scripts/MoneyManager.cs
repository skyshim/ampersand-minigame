using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Money : MonoBehaviour
{
    public TextMeshProUGUI Moneytext;

    private float MoneyNum = 0f;
    private float MoneyValue = 1f;

    //Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MoneyNum += MoneyValue;
            Moneytext.text = "Money: " + MoneyNum;
        }
    }
}
