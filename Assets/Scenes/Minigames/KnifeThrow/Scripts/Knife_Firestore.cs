using Firebase.Auth;
using Firebase.Firestore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Knife_Firestore : MonoBehaviour
{
    FirebaseAuth auth;
    FirebaseFirestore db;

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        db = FirebaseFirestore.DefaultInstance;
    }

    public async Task SaveScore(string gameId, int score)
    {
        if (auth.CurrentUser == null)
        {
            Debug.LogWarning("�α��� �ʿ�!");
            return;
        }

        string uid = auth.CurrentUser.UserId;
        string nickname = auth.CurrentUser.DisplayName ?? "Guest";

        DocumentReference docRef = db.Collection("gameScores")
                                     .Document(gameId)
                                     .Collection("records")
                                     .Document(uid);

        // Firestore�� ������ ������
        var data = new
        {
            score = score,
            nickname = nickname,
            updatedAt = Timestamp.GetCurrentTimestamp()
        };

        try
        {
            // �̹� ������ ������ ���ؼ� ����
            DocumentSnapshot snap = await docRef.GetSnapshotAsync();
            if (snap.Exists && snap.ContainsField("highScore"))
            {
                int oldScore = snap.GetValue<int>("highScore");
                if (score > oldScore)
                {
                    await docRef.SetAsync(data, SetOptions.MergeAll);
                    Debug.Log("�ű�� ���� �Ϸ�!");
                }
                else
                {
                    Debug.Log("���� ����� �� ����. ���� �� ��.");
                }
            }
            else
            {
                await docRef.SetAsync(data);
                Debug.Log("ù ��� ���� �Ϸ�!");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("���� ���� ����: " + e.Message);
        }
    }
}
