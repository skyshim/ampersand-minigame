using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM : MonoBehaviour {

    public GameObject fruitprefab;

    [HideInInspector]
    public List<int> fruitPreview = new List<int> { 0, 0, 0, 0 };
    public bool updatePreview = false;
    public bool mergeSig = false;
    public Vector2 mergePos;

    // Start is called before the first frame update
    void Start() {
        for (int i = 0; i < 4; i++) {
            int r = Random.Range(1, 6);
            if (r <= 3) {
                fruitPreview[i] = 1;
            }
            else if (r <= 5) {
                fruitPreview[i] = 2;
            }
            else if (r == 6) {
                fruitPreview[i] = 3;
            }
        }

    }

    // Update is called once per frame
    void Update() {
        if (updatePreview) {
            for (int i = 0; i < 3; i++) {
                fruitPreview[i] = fruitPreview[i + 1];
            }
            int r = Random.Range(1, 6);
            if (r <= 3) {
                fruitPreview[3] = 1;
            }
            else if (r <= 5) {
                fruitPreview[3] = 2;
            }
            else if (r == 6) {
                fruitPreview[3] = 3;
            }
            updatePreview = false;
        }

        if (mergeSig) {
            Instantiate(fruitprefab, mergePos, Quaternion.identity);
            mergeSig = false;
        }
    }


}
