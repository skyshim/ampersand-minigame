using Firebase;
using Firebase.Auth;
using UnityEngine;

public class FirebaseInit : MonoBehaviour
{
    public static FirebaseAuth auth;
    public static FirebaseUser user;

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var status = task.Result;
            if (status == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
                Debug.Log("Firebase Auth initialized!");
            }
            else
            {
                Debug.LogError("Firebase dependency error: " + status);
            }
        });
    }
}
