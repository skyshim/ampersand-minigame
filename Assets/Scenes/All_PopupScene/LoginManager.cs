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
        // Firebase ������ üũ
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
                firebaseReady = true;
                Debug.Log("Firebase �ʱ�ȭ �Ϸ�");
            }
            else
            {
                Debug.LogError("Firebase �ʱ�ȭ ����: " + task.Result);
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                    toastMessage.ShowToast("Firebase �ʱ�ȭ ����", Color.red)
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
            toastMessage.ShowToast("Firebase �ʱ�ȭ ��...", Color.yellow);
            return;
        }

        string email = idInputField.text + "@dummy.com";
        string password = pwInputField.text;

        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogWarning("�α��� ��ҵ�");
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                    toastMessage.ShowToast("�α��� ��ҵ�", Color.red)
                );
                return;
            }

            if (task.IsFaulted)
            {
                foreach (var e in task.Exception.Flatten().InnerExceptions)
                {
                    FirebaseException fe = e as FirebaseException;
                    string msg = fe != null ? $"�ڵ� {fe.ErrorCode}: {fe.Message}" : e.Message;
                    Debug.LogError("�α��� ����: " + msg);

                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                        toastMessage.ShowToast("�α��� ����: " + msg, Color.red)
                    );
                }
                return;
            }

            Debug.Log("�α��� ����!");
            OnLoginSuccess?.Invoke();
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
                toastMessage.ShowToast("�α��� ����!", Color.green)
            );
        });
    }

    void HandleSignUp()
    {
        if (!firebaseReady)
        {
            toastMessage.ShowToast("Firebase �ʱ�ȭ ��...", Color.yellow);
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
                    string msg = fe != null ? $"�ڵ� {fe.ErrorCode}: {fe.Message}" : e.Message;
                    Debug.LogError("ȸ������ ����: " + msg);

                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                        toastMessage.ShowToast("ȸ������ ����: " + msg, Color.red)
                    );
                }
                return;
            }

            Debug.Log("ȸ������ ����!");
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
                toastMessage.ShowToast("ȸ������ ����!", Color.green)
            );
        });
    }

    void ClosePopup()
    {
        loginPopup.SetActive(false);
    }
}
