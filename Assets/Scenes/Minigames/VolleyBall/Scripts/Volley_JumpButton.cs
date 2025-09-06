using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Volley_JumpButton : MonoBehaviour
{
    private Image image;
    public Sprite jumpSprite;
    public Sprite spikeSprite;

    public Volley_PlayerMove playerMove;

    private bool isGround = false;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        isGround = playerMove.GroundCheck();
        if (isGround )
        {
            image.sprite = jumpSprite;
        }
        else
        {
            image.sprite = spikeSprite;
        }
    }
}
