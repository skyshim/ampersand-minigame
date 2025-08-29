using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Knife_BtnManager : MonoBehaviour
{
    public void OnRestartClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void OnHomeClick()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
