using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DinoJumpManagerScript : MonoBehaviour
{
    public GameObject dino;
    public TMP_Text gametext;

    public float gameSpeed = 1f;
    public bool isGamestarted = false;
    public bool isGameovered = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 || Input.GetMouseButton(0))
        {
            gametext.gameObject.SetActive(false);
            DinoScript dinoScript = dino.GetComponent<DinoScript>();
            dinoScript.jump();
            isGamestarted = true;
        }
        if (!isGameovered && isGamestarted)
        {
            gameSpeed += Time.deltaTime * Time.deltaTime;
        }
    }
}
