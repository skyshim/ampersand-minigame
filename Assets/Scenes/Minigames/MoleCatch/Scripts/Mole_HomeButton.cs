using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mole_HomeButton : MonoBehaviour
{
    public void ExitGame()
    {

        SceneManager.LoadScene("MainMenu");
    }
}
