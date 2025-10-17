using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_1945 : MonoBehaviour
{
    public float speed = 5f;

    private GM_1945 gm;
    // Start is called before the first frame update
    void Start()
    {
        gm = FindAnyObjectByType<GM_1945>();

    }

    // Update is called once per frame
    void Update()
    {
        float xInput = Input.GetAxisRaw("Horizontal");
        if (xInput == 1 && transform.position.x < 2.5f)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
        else if (xInput == -1 && transform.position.x > -2.5f)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
    }

    void FireBullet(int bpm) {
        
    }

}
