using UnityEngine;

public enum MoleType
{
    RedMole,
    BlueMole,
    RedBomb,
    BlueBomb
}

public class Mole : MonoBehaviour
{
    public MoleType moleType;
    private MoleSpawner spawner;

    private void Start()
    {
        spawner = FindObjectOfType<MoleSpawner>();
    }

    private void Update()
    {
        // ��ġ�� ���� ��
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
                Vector2 touchPos2D = new Vector2(touchPos.x, touchPos.y);

                RaycastHit2D hit = Physics2D.Raycast(touchPos2D, Vector2.zero);
                if (hit.collider != null && hit.collider.gameObject == this.gameObject)
                {
                    HandleHit();
                }
            }
        }
    }

    private void OnMouseDown()
    {
        HandleHit();
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

        GameManager.Instance.AddScore(moleType, scoreChange);
        Destroy(gameObject);
    }
}
