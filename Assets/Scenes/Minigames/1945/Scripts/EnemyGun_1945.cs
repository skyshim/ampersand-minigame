using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun_1945 : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private GM_1945 gm;

    private float fireTimer = 0f;
    private float interval;

    public float speed = 6f;
    public float bulletRange;

    private void Start() {
        gm = FindObjectOfType<GM_1945>();
        interval = 60f / gm.bpm;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;

        fireTimer += Time.deltaTime;

        //발사 타이밍 도달 시
        if (fireTimer >= interval) {
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
        float rad = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
        GameObject a = Instantiate(bullet, transform.position + new Vector3(bulletRange * Mathf.Cos(rad), bulletRange * Mathf.Sin(rad), 1), transform.rotation);
        GameObject b = Instantiate(bullet, transform.position - new Vector3(bulletRange * Mathf.Cos(rad), bulletRange * Mathf.Sin(rad), 1), transform.rotation);
        a.tag = "bullet-p_1945"; a.GetComponent<Bullet_1945>().whoSpawn = "enemy_Gun";
        b.tag = "bullet-p_1945"; b.GetComponent<Bullet_1945>().whoSpawn = "enemy_Gun";
    }
}
