using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateKnife : MonoBehaviour
{
    public KnifeUI knifeUI;
    public RoundManager roundManager;
    [SerializeField] private GameObject Knife;
    private GameObject currentKnife;

    private bool isReadyToThrow = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isReadyToThrow) return;

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
        if (!roundManager.CanThrowKnife())
        {
            Debug.Log("더이상 칼을 던질 수 없습니다.");
            return;
        }
        currentKnife.GetComponent<KnifeControl>().Launch();
        isReadyToThrow = false;
        knifeUI.UseKnife(); 
    }

    public void SpawnKnife()
    {
        if (!roundManager.CanThrowKnife()) return;
        currentKnife = Instantiate(Knife, new Vector3(0f, -4f, 0f), Quaternion.identity);
        Debug.Log(currentKnife != null);
        isReadyToThrow = true;
    }
}
