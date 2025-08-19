using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife_RoundResult : MonoBehaviour
{
    public GameObject clearPanel;
    public GameObject failPanel;

    public void ShowClear()
    {
        clearPanel.SetActive(true);
    }
    public void ShowFail()
    {
        failPanel.SetActive(true);
    }
    public void HideAll()
    {
        clearPanel.SetActive(false);
        failPanel.SetActive(false);
    }
}
