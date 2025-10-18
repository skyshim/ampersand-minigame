using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun_1945 : MonoBehaviour
{
    [SerializeField] private GameObject bullet;

    private float fireTimer = 0f;
    public float interval = 1f;

    public float speed = 6f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;

        fireTimer += Time.deltaTime;

        // 발사 타이밍 도달 시
        if (fireTimer >= interval / 4) {
            FireBullet();
            fireTimer = 0f; // 타이머 초기화
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "boundary_1945" || collision.gameObject.tag == "bullet-p_1945") {
            Destroy(gameObject);
        }
    }


    private void FireBullet() {
        GameObject a = Instantiate(bullet, transform.position + new Vector3(-0.3f, 0.5f, 1), transform.rotation);
        GameObject b = Instantiate(bullet, transform.position + new Vector3(+0.3f, 0.5f, 1), transform.rotation);
        a.tag = "bullet-p_1945";
        b.tag = "bullet-p_1945";
    }
}
