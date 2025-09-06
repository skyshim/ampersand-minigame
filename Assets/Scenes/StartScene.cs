using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    public TMP_Text AnneText;


    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 클릭 또는 터치
        {

            SceneManager.LoadScene("MainMenu");
 
        }
    }

}
