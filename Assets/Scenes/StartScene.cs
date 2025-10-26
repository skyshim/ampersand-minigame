using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    public TMP_Text AnneText;
    public GameObject understandPanel;
    public bool doYouUnderstand = false;

    private void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
    }
    void Update() {
        if (Input.GetMouseButtonDown(0))
        {
            if (doYouUnderstand)
            {
                SceneManager.LoadScene("MainMenu");
            }
            else
            {
                doYouUnderstand = true;
                understandPanel.SetActive(true);
            }
            
        }
    }
}
