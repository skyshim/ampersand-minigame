using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnBtn : MonoBehaviour
{
    public void OnReturnClick()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
