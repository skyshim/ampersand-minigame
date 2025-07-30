using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateKnife : MonoBehaviour
{
    public KnifeUI knifeUI;
    public RoundManager roundManager;
    [SerializeField] private GameObject Knife;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            ThrowSelf();
        }
        if (Input.GetMouseButtonDown(0))
        {
            ThrowSelf();
        }
    }
    private void ThrowSelf()
    {   
        Instantiate(Knife, new Vector3(0f, -4f, 0f), Quaternion.identity);
        knifeUI.UseKnife();
        roundManager.ThrowKnifeCount();
    }
}
