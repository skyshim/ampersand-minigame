using System.Collections.Generic;
using UnityEngine;

public enum MoleType
{
    RedMole,
    BlueMole,
    RedBomb,
    BlueBomb
}

public class Mole_Mole : MonoBehaviour
{
    public MoleType moleType;
    private Mole_MoleSpawner spawner;
    Animator animator;

    private bool isTouched = false;

    private void Start()
    {
        spawner = FindObjectOfType<Mole_MoleSpawner>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Vector2 inputPos = Vector2.zero;
        bool inputDetected = false;

        // ����� ��ġ
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                inputPos = Camera.main.ScreenToWorldPoint(touch.position);
                inputDetected = true;
            }
        }
        // PC ���콺 Ŭ��
        else if (Input.GetMouseButtonDown(0))
        {
            inputPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            inputDetected = true;
        }

        if (inputDetected)
        {
            Vector2 inputPos2D = new Vector2(inputPos.x, inputPos.y);
            RaycastHit2D hit = Physics2D.Raycast(inputPos2D, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == this.gameObject && !isTouched)
            {
                if (animator != null)
                    animator.SetTrigger("OnClick");

                HandleHit();
            }
        }
    }

    private void HandleHit()
    {
        int scoreChange = 0;
        switch (moleType)
        {
            case MoleType.RedMole:
            case MoleType.BlueMole:
                scoreChange = 2; // �δ��� +2��
                break;
            case MoleType.RedBomb:
            case MoleType.BlueBomb:
                scoreChange = -1; // ��ź -1��
                break;
        }

        Mole_GameManager.Instance.AddScore(moleType, scoreChange);
        isTouched = true;
        Destroy(gameObject, 0.3f);
    }
}