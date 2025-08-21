using System;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using TMPro;
using System.Collections;

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
            if (idInputField.text.Contains("@"))
            {
                toastMessage.ShowToast("���̵𿡴� @�� ���� ������.", Color.red);
                return;
            }

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
                        string friendlyMsg = "";

                        if (fe != null)
                        {
                            switch (fe.ErrorCode)
                            {
                                case (int)AuthError.InvalidEmail:
                                    friendlyMsg = "���̵� ������ �߸��Ǿ����. �ùٸ� ���̵� �Է����ּ���.";
                                    break;
                                case (int)AuthError.WrongPassword:
                                    friendlyMsg = "��й�ȣ�� Ʋ�Ƚ��ϴ�. �ٽ� Ȯ�����ּ���.";
                                    break;
                                case (int)AuthError.UserNotFound:
                                    friendlyMsg = "�������� �ʴ� �����̿���. ȸ������ ���� ���ּ���.";
                                    break;
                                case (int)AuthError.EmailAlreadyInUse:
                                    friendlyMsg = "�̹� ���Ե� �����Դϴ�. �ٸ� ���̵� ������ּ���.";
                                    break;
                                case (int)AuthError.WeakPassword:
                                    friendlyMsg = "��й�ȣ�� �ʹ� ���ؿ�. 6�� �̻����� �Է����ּ���.";
                                    break;
                                default:
                                    friendlyMsg = "��й�ȣ�� ���� �ʰų�, �� �� ���� ������ �߻��߽��ϴ�. ��� �� �ٽ� �õ����ּ���.";
                                    break;
                            }
                        }
                        else
                        {
                            friendlyMsg = e.Message;
                        }
                        Debug.LogError("�α��� ����: " + friendlyMsg);

                        UnityMainThreadDispatcher.Instance().Enqueue(() =>
                            toastMessage.ShowToast("�α��� ����: " + friendlyMsg, Color.red)
                        );
                    }
                    return;
                }

                Debug.Log("�α��� ����!");
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {

                    toastMessage.ShowToast("�α��� ����!", Color.green);
                    StartCoroutine(ClosePopupDelayed(1f));
                });
                OnLoginSuccess?.Invoke();
            });
        }

    private IEnumerator ClosePopupDelayed(float delay)
    {           
        yield return new WaitForSeconds(delay);
        ClosePopup(); // ���� �˾� �ݴ� �Լ�
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
                    string friendlyMsg = "";

                    if (fe != null)
                    {
                        switch (fe.ErrorCode)
                        {
                            case (int)AuthError.EmailAlreadyInUse:
                                friendlyMsg = "�̹� ���Ե� �����̿���. �ٸ� ���̵� ������ּ���.";
                                break;
                            case (int)AuthError.InvalidEmail:
                                friendlyMsg = "���̵� ������ �߸��Ǿ����. �ùٸ� ���̵� �Է����ּ���.";
                                break;
                            case (int)AuthError.WeakPassword:
                                friendlyMsg = "��й�ȣ�� �ʹ� ���ؿ�. 6�� �̻����� �Է����ּ���.";
                                break;
                            case (int)AuthError.OperationNotAllowed:
                                friendlyMsg = "�̸���/��й�ȣ ���� ������ ���� ��Ȱ��ȭ�Ǿ� �ֽ��ϴ�.";
                                break;
                            default:
                                friendlyMsg = "ȸ������ �� �� �� ���� ������ �߻��߽��ϴ�. ��� �� �ٽ� �õ����ּ���.";
                                break;
                        }
                    }
                    else
                    {
                        friendlyMsg = e.Message;
                    }

                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                        toastMessage.ShowToast(friendlyMsg, Color.red)
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
