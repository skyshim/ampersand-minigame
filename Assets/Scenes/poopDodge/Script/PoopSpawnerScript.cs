using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoopSpawnerScript : MonoBehaviour
{
    public Rigidbody2D rb;
    public float time = 1f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
