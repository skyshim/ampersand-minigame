using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ThingSpawnerScript : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameObject manager;
    public GameObject things;
    public float spawnRate = 1f;
    public float afterSpawn = 0;
    public float minSpawn = 1f, maxSpawn = 2f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        DinoJumpManagerScript managerScript = manager.GetComponent<DinoJumpManagerScript>();

        if (!managerScript.isGameovered && managerScript.isGamestarted)
        {

            afterSpawn += Time.deltaTime;
            if (afterSpawn > spawnRate)
            {
                afterSpawn = 0f;
                GameObject Thing = Instantiate(things, transform.position, transform.rotation);

                if (maxSpawn > 1.3) { maxSpawn -= Time.deltaTime * 10; }    
                spawnRate = Random.Range(minSpawn, maxSpawn);


            }
        }
    }
}
