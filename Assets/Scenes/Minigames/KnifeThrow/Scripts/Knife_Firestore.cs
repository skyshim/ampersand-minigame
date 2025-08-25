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
            Debug.LogWarning("로그인 필요!");
            return;
        }

        string uid = auth.CurrentUser.UserId;
        string nickname = auth.CurrentUser.DisplayName ?? "Guest";

        DocumentReference docRef = db.Collection("gameScores")
                                     .Document(gameId)
                                     .Collection("records")
                                     .Document(uid);

        // Firestore에 저장할 데이터
        var data = new
        {
            score = score,
            nickname = nickname,
            updatedAt = Timestamp.GetCurrentTimestamp()
        };

        try
        {
            // 이미 점수가 있으면 비교해서 갱신
            DocumentSnapshot snap = await docRef.GetSnapshotAsync();
            if (snap.Exists && snap.ContainsField("highScore"))
            {
                int oldScore = snap.GetValue<int>("highScore");
                if (score > oldScore)
                {
                    await docRef.SetAsync(data, SetOptions.MergeAll);
                    Debug.Log("신기록 저장 완료!");
                }
                else
                {
                    Debug.Log("기존 기록이 더 높음. 저장 안 함.");
                }
            }
            else
            {
                await docRef.SetAsync(data);
                Debug.Log("첫 기록 저장 완료!");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("점수 저장 실패: " + e.Message);
        }
    }
}
