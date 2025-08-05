using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PoopSpawnerScript : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameObject poops;
    public GameObject manager;

    public float time = 1f;
    public float afterSpawn = 0f;
    public float spawnRate;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spawnRate = Random.Range(0.5f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        GameManagerScript managerScript = manager.GetComponent<GameManagerScript>();

        if (managerScript.isGameStart && !managerScript.isGameOver && managerScript.gamemode == 1)
        {
            afterSpawn += Time.deltaTime;
            if (afterSpawn > spawnRate)
            {
                afterSpawn = 0f;
                transform.position = new Vector3(Random.Range(-4, 4), 8, 0);
                GameObject poop = Instantiate(poops, transform.position, transform.rotation);

                spawnRate = Random.Range(0f, 0.7f);
            }
        }
    }
}
