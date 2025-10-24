using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player_1945 : MonoBehaviour 
{
    private GM_1945 gm;
    [SerializeField] private GameObject bullet;

    public float speed = 5f;
    public int gunlevel = 1;
    public int missilelevel = 0;
    public int hp = 1000;

    private float fireTimer;
    private bool isFiring = false;

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


        float yinput  = Input.GetAxisRaw("Vertical");
        if (yinput == 1 && transform.position.y < 4.7f)
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        }
        else if (yinput == -1 && transform.position.y > -4.8f)
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);
        }


        if (Input.GetKeyDown(KeyCode.Space)) {
            isFiring = !isFiring;
        }


        float interval = 60f / gm.bpm;
        if (isFiring) {
            // 타이머 갱신
            fireTimer += Time.deltaTime;

            // 발사 타이밍 도달 시
            if (fireTimer >= interval/4) {
                Fire_Bullet(gunlevel);
                fireTimer = 0f; // 타이머 초기화
            }
        }
    }

    private void Fire_Bullet(int level) {
        if (level == 1) {
            GameObject a = Instantiate(bullet, transform.position + new Vector3(0, 0.5f, 1), Quaternion.Euler(0, 0, 0));
            a.tag = "bullet-p_1945"; a.GetComponent<Bullet_1945>().whoSpawn = "player";
        }
        else if (level == 2) {
            GameObject a = Instantiate(bullet, transform.position + new Vector3(-0.3f, 0.5f, 1), Quaternion.Euler(0, 0, 0));
            GameObject b = Instantiate(bullet, transform.position + new Vector3(+0.3f, 0.5f, 1), Quaternion.Euler(0, 0, 0));
            a.tag = "bullet-p_1945"; a.GetComponent<Bullet_1945>().whoSpawn = "player";
            b.tag = "bullet-p_1945"; b.GetComponent<Bullet_1945>().whoSpawn = "player";
        }
        else if (level == 3) {
            GameObject a = Instantiate(bullet, transform.position + new Vector3(-0.3f, 0.2f, 1), Quaternion.Euler(0, 0, 15));
            GameObject b = Instantiate(bullet, transform.position + new Vector3(0, 0.5f, 1), Quaternion.Euler(0, 0, 0));
            GameObject c = Instantiate(bullet, transform.position + new Vector3(+0.3f, 0.2f, 1), Quaternion.Euler(0, 0, -15));
            a.tag = "bullet-p_1945"; a.GetComponent<Bullet_1945>().whoSpawn = "player";
            b.tag = "bullet-p_1945"; b.GetComponent<Bullet_1945>().whoSpawn = "player";
            c.tag = "bullet-p_1945";
        }
        else if (level == 4) {
            GameObject a = Instantiate(bullet, transform.position + new Vector3(-0.3f, 0.2f, 1), Quaternion.Euler(0, 0, 0));
            GameObject b = Instantiate(bullet, transform.position + new Vector3(-0.1f, 0.5f, 1), Quaternion.Euler(0, 0, 0));
            GameObject c = Instantiate(bullet, transform.position + new Vector3(+0.1f, 0.5f, 1), Quaternion.Euler(0, 0, 0));
            GameObject d = Instantiate(bullet, transform.position + new Vector3(+0.3f , 0.2f, 1), Quaternion.Euler(0, 0, 0));
            a.tag = "bullet-p_1945"; a.GetComponent<Bullet_1945>().whoSpawn = "player";
            b.tag = "bullet-p_1945"; b.GetComponent<Bullet_1945>().whoSpawn = "player";
            c.tag = "bullet-p_1945"; c.GetComponent<Bullet_1945>().whoSpawn = "player";
            d.tag = "bullet-p_1945"; d.GetComponent<Bullet_1945>().whoSpawn = "player";
        }
        else if (level == 5) {
            GameObject a = Instantiate(bullet, transform.position + new Vector3(-0.4f, 0.2f, 1), Quaternion.Euler(0, 0, 20));
            GameObject b = Instantiate(bullet, transform.position + new Vector3(-0.2f, 0.2f, 1), Quaternion.Euler(0, 0, 10));
            GameObject c = Instantiate(bullet, transform.position + new Vector3(0, 0.2f, 1), Quaternion.Euler(0, 0, 0));
            GameObject d = Instantiate(bullet, transform.position + new Vector3(+0.2f, 0.2f, 1), Quaternion.Euler(0, 0, -10));
            GameObject e = Instantiate(bullet, transform.position + new Vector3(+0.4f, 0.2f, 1), Quaternion.Euler(0, 0, -20));
            a.tag = "bullet-p_1945"; a.GetComponent<Bullet_1945>().whoSpawn = "player";
            b.tag = "bullet-p_1945"; b.GetComponent<Bullet_1945>().whoSpawn = "player";
            c.tag = "bullet-p_1945"; c.GetComponent<Bullet_1945>().whoSpawn = "player";
            d.tag = "bullet-p_1945"; d.GetComponent<Bullet_1945>().whoSpawn = "player";
            e.tag = "bullet-p_1945"; e.GetComponent<Bullet_1945>().whoSpawn = "player";
        }
    }

}
