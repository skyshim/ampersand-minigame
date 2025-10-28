using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class to50_GameManager : MonoBehaviour
{
    public Button buttonPrefab;
    public Transform buttonParent;
    public to50_Timer timer;
    public Image endPanel;
    public Image startPanel;
    public GameObject rankingPanel;
    public TMP_Text countdownText;

    public List<Button> buttons = new List<Button>();
    public List<TMP_Text> texts = new List<TMP_Text>();

    private List<int> firstNumbers = Enumerable.Range(1, 25).ToList();
    private List<int> secondNumbers = Enumerable.Range(26, 25).ToList();

    public int currentNumber = 1;
    private int num;
    public bool isStarted = false;

    private void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;

        rankingPanel.SetActive(false);
        firstNumbers.Shuffle();
        secondNumbers.Shuffle();
        GenerateButtons();
        StartCoroutine(StartCountdown());
    }

    private IEnumerator StartCountdown()
    {
        float currentTime = 3f;

        while (currentTime > 0)
        {
            countdownText.text = Mathf.Ceil(currentTime).ToString(); // 3,2,1 표시
            yield return new WaitForSeconds(0.5f);
            currentTime -= 1f;
        }

        countdownText.text = "Go!";
        yield return new WaitForSeconds(0.5f);

        startPanel.gameObject.SetActive(false); // 텍스트 숨기기

        // 게임 시작 함수 호출
        isStarted = true;
    }

    void GenerateButtons()
    {
        // 기존 버튼 정리
        foreach (Transform child in buttonParent)
            Destroy(child.gameObject);

        buttons.Clear();
        texts.Clear();

        // 버튼 생성 및 등록
        for (int i = 0; i < 25; i++)
        {
            Button newButton = Instantiate(buttonPrefab, buttonParent);
            newButton.name = "Button" + (i + 1);

            TMP_Text textComponent = newButton.GetComponentInChildren<TMP_Text>();
            
            if (textComponent != null)
            {
                to50_Button eachBtn = newButton.GetComponent<to50_Button>();
                num = firstNumbers[firstNumbers.Count - 1];
                firstNumbers.RemoveAt(firstNumbers.Count - 1);
                textComponent.text = num.ToString();
                eachBtn.num = num;
                
                texts.Add(textComponent);
                newButton.onClick.AddListener(() => eachBtn.OnClicked());
            }

            buttons.Add(newButton);
        }

        Debug.Log($"총 버튼 개수: {buttons.Count}, 텍스트 개수: {texts.Count}");
    }

    public void EndGame()
    {
        timer.StopTimer();
        endPanel.gameObject.SetActive(true);
        rankingPanel.SetActive(true);
    }

    public bool isCurrent(int input)
    {
        return (input == currentNumber);
    }

    public int popSecond()
    {
        int temp = secondNumbers[secondNumbers.Count - 1];
        secondNumbers.RemoveAt(secondNumbers.Count - 1);
        return temp;
    }
}

public static class ListExtensions  // ① static 클래스
{
    public static void Shuffle<T>(this IList<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rnd = UnityEngine.Random.Range(i, list.Count);
            (list[i], list[rnd]) = (list[rnd], list[i]);
        }
    }
}