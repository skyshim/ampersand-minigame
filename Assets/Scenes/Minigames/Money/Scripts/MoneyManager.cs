using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Money : MonoBehaviour, IPointerClickHandler
{
    public TextMeshProUGUI Moneytext;

    private float MoneyNum = 0f;
    private float MoneyValue = 1000f;

    public void OnPointerClick(PointerEventData eventData)
    {
        MoneyNum += MoneyValue;
        Moneytext.text = "µ·: " + MoneyNum;
    }
}
