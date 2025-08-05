using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class PoopSpawnerScript : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameObject poops;
    public GameObject manager;
    public Slider controller;

    public float time = 1f;
    public float afterSpawn = 0f;
    public float spawnRate;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spawnRate = Random.Range(0.5f, 1f);
        controller.onValueChanged.AddListener(move);
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
                transform.position = new Vector3(Random.Range(-4, 5), 8, 0);
                GameObject poop = Instantiate(poops, transform.position, transform.rotation);

                spawnRate = Random.Range(0.3f, 0.7f);
            }
        }
        else if (managerScript.isGameStart && !managerScript.isGameOver && managerScript.gamemode == 2)
        {
            controller.gameObject.SetActive(true);
            rb.position = new Vector2(rb.position.x, 5);
            
            afterSpawn += Time.deltaTime;
            if (afterSpawn > spawnRate)
            {
                afterSpawn = 0f;
                GameObject poop = Instantiate(poops, transform.position, transform.rotation);

                spawnRate = Random.Range(0.5f, 1.5f);
            }
        }
    }
    
    void move(float value)
    {
        rb.position = new Vector2(value, 5);
    }
}
