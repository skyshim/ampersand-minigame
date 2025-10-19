using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake_StartBtn : MonoBehaviour
{
    public Snake_GameManager gameManager;

    public void OnClick9x9() { gameManager.StartGame(9); }
    public void OnClick12x12() { gameManager.StartGame(12); }
    public void OnClick15x15() { gameManager.StartGame(15); }
}
