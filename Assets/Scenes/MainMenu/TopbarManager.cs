using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopbarManager : MonoBehaviour
{
    public void OnSettingsClicked()
    {
        // ���� �г� ���� or �� ��ȯ
        Debug.Log("���� ��ư Ŭ��");
        // ��: UIManager.Instance.ShowSettings();
    }

    public void OnRankingClicked()
    {
        Debug.Log("��ŷ ��ư Ŭ��");
        // ��ŷ ȭ�� ����
        // ��: UIManager.Instance.ShowRanking();
    }

    public void OnExitClicked()
    {
        Debug.Log("������ ��ư Ŭ��");
        Application.Quit(); // ����� ���ӿ��� ����
        // �����Ϳ����� �۵� �� ��
    }
}
