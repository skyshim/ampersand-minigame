using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    public TMP_Text AnneText;

    private void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
    }
    void Update() {
        if (Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
