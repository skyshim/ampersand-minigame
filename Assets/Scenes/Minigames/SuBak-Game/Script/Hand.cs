using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    private float go = 0;
    private int pickedfruitLevel = 0;

    public GameObject fruitPrefab;
    private GM gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GM>();
        
    }

    // Update is called once per frame
    void Update()
    {
        // 이동 명령
        go = Input.GetAxisRaw("Horizontal");
        transform.Translate(new Vector2(go, 0) * Time.deltaTime * 5);


        if (Input.GetKeyDown(KeyCode.Space)) {
            pickedfruitLevel = gm.fruitPreview[1]; // 다음 과일 선택
            Instantiate(fruitPrefab, transform.position, transform.rotation); // 이후 과일 생성
        }
    }
}
