using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��׶��� �����忡�� Unity ���� ������� �����ϰ� �ڵ� ����
/// </summary>
public class UnityMainThreadDispatcher : MonoBehaviour
{
    private static readonly Queue<Action> _executionQueue = new Queue<Action>();
    private static UnityMainThreadDispatcher _instance;

    public static UnityMainThreadDispatcher Instance()
    {
        if (!_instance)
        {
            _instance = FindObjectOfType<UnityMainThreadDispatcher>();
            if (!_instance)
            {
                GameObject obj = new GameObject("MainThreadDispatcher");
                _instance = obj.AddComponent<UnityMainThreadDispatcher>();
                DontDestroyOnLoad(obj);
            }
        }
        return _instance;
    }

    /// <summary>
    /// ���� �����忡�� ������ �׼� ť�� �߰�
    /// </summary>
    public void Enqueue(Action action)
    {
        if (action == null) return;
        lock (_executionQueue)
        {
            _executionQueue.Enqueue(action);
        }
    }

    void Update()
    {
        lock (_executionQueue)
        {
            while (_executionQueue.Count > 0)
            {
                try
                {
                    _executionQueue.Dequeue().Invoke();
                }
                catch (Exception e)
                {
                    Debug.LogError("MainThreadDispatcher ���� �� ����: " + e);
                }
            }
        }
    }
}
