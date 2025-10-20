using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake_StartBtn : MonoBehaviour
{
    public Snake_GameManager gameManager;

    public void OnClick9x9() { gameManager.StartGame(9); }
    public void OnClick13x13() { gameManager.StartGame(13); }
    public void OnClick17x17() { gameManager.StartGame(17); }
}
