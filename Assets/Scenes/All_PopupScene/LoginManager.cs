using System;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using TMPro;

public class LoginManager : MonoBehaviour
{
    [Header("UI References")]
    public InputField idInputField;
    public InputField pwInputField;
    public Button loginButton;
    public Button signupButton;
    public Button closeButton;
    public ToastMessage toastMessage;
    public GameObject loginPopup;

    private FirebaseAuth auth;
    private bool firebaseReady = false;

    public event System.Action OnLoginSuccess;

    void Awake()
    {

        auth = FirebaseAuth.DefaultInstance;
        // Firebase 의존성 체크
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
                firebaseReady = true;
                Debug.Log("Firebase 초기화 완료");
            }
            else
            {
                Debug.LogError("Firebase 초기화 실패: " + task.Result);
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                    toastMessage.ShowToast("Firebase 초기화 실패", Color.red)
                );
            }
        });
    }

    void Start()
    {
        loginButton.onClick.AddListener(HandleLogin);
        signupButton.onClick.AddListener(HandleSignUp);
        closeButton.onClick.AddListener(ClosePopup);
    }

    void HandleLogin()
    {
        if (!firebaseReady)
        {
            toastMessage.ShowToast("Firebase 초기화 중...", Color.yellow);
            return;
        }

        string email = idInputField.text + "@dummy.com";
        string password = pwInputField.text;

        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogWarning("로그인 취소됨");
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                    toastMessage.ShowToast("로그인 취소됨", Color.red)
                );
                return;
            }

            if (task.IsFaulted)
            {
                foreach (var e in task.Exception.Flatten().InnerExceptions)
                {
                    FirebaseException fe = e as FirebaseException;
                    string msg = fe != null ? $"코드 {fe.ErrorCode}: {fe.Message}" : e.Message;
                    Debug.LogError("로그인 실패: " + msg);

                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                        toastMessage.ShowToast("로그인 실패: " + msg, Color.red)
                    );
                }
                return;
            }

            Debug.Log("로그인 성공!");
            OnLoginSuccess?.Invoke();
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
                toastMessage.ShowToast("로그인 성공!", Color.green)
            );
        });
    }

    void HandleSignUp()
    {
        if (!firebaseReady)
        {
            toastMessage.ShowToast("Firebase 초기화 중...", Color.yellow);
            return;
        }

        string email = idInputField.text + "@dummy.com";
        string password = pwInputField.text;

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                foreach (var e in task.Exception.Flatten().InnerExceptions)
                {
                    FirebaseException fe = e as FirebaseException;
                    string msg = fe != null ? $"코드 {fe.ErrorCode}: {fe.Message}" : e.Message;
                    Debug.LogError("회원가입 실패: " + msg);

                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                        toastMessage.ShowToast("회원가입 실패: " + msg, Color.red)
                    );
                }
                return;
            }

            Debug.Log("회원가입 성공!");
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
                toastMessage.ShowToast("회원가입 성공!", Color.green)
            );
        });
    }

    void ClosePopup()
    {
        loginPopup.SetActive(false);
    }
}
