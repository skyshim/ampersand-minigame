using UnityEngine;
using UnityEngine.SceneManagement;

public class GoMenuScript : MonoBehaviour
{
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}