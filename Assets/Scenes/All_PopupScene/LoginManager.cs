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
            if (idInputField.text.Contains("@"))
            {
                toastMessage.ShowToast("아이디에는 @를 넣지 마세요.", Color.red);
                return;
            }

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
                        string friendlyMsg = "";

                        if (fe != null)
                        {
                            switch (fe.ErrorCode)
                            {
                                case (int)AuthError.InvalidEmail:
                                    friendlyMsg = "아이디 형식이 잘못되었어요. 올바른 아이디를 입력해주세요.";
                                    break;
                                case (int)AuthError.WrongPassword:
                                    friendlyMsg = "비밀번호가 틀렸습니다. 다시 확인해주세요.";
                                    break;
                                case (int)AuthError.UserNotFound:
                                    friendlyMsg = "존재하지 않는 계정이에요. 회원가입 먼저 해주세요.";
                                    break;
                                case (int)AuthError.EmailAlreadyInUse:
                                    friendlyMsg = "이미 가입된 계정입니다. 다른 아이디를 사용해주세요.";
                                    break;
                                case (int)AuthError.WeakPassword:
                                    friendlyMsg = "비밀번호가 너무 약해요. 6자 이상으로 입력해주세요.";
                                    break;
                                default:
                                    friendlyMsg = "비밀번호가 맞지 않거나, 알 수 없는 오류가 발생했습니다. 잠시 후 다시 시도해주세요.";
                                    break;
                            }
                        }
                        else
                        {
                            friendlyMsg = e.Message;
                        }
                        Debug.LogError("로그인 실패: " + friendlyMsg);

                        UnityMainThreadDispatcher.Instance().Enqueue(() =>
                            toastMessage.ShowToast("로그인 실패: " + friendlyMsg, Color.red)
                        );
                    }
                    return;
                }

                Debug.Log("로그인 성공!");
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {

                    toastMessage.ShowToast("로그인 성공!", Color.green);
                    StartCoroutine(ClosePopupDelayed(1f));
                });
                OnLoginSuccess?.Invoke();
            });
        }

    private IEnumerator ClosePopupDelayed(float delay)
    {           
        yield return new WaitForSeconds(delay);
        ClosePopup(); // 실제 팝업 닫는 함수
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
                    string friendlyMsg = "";

                    if (fe != null)
                    {
                        switch (fe.ErrorCode)
                        {
                            case (int)AuthError.EmailAlreadyInUse:
                                friendlyMsg = "이미 가입된 계정이에요. 다른 아이디를 사용해주세요.";
                                break;
                            case (int)AuthError.InvalidEmail:
                                friendlyMsg = "아이디 형식이 잘못되었어요. 올바른 아이디를 입력해주세요.";
                                break;
                            case (int)AuthError.WeakPassword:
                                friendlyMsg = "비밀번호가 너무 약해요. 6자 이상으로 입력해주세요.";
                                break;
                            case (int)AuthError.OperationNotAllowed:
                                friendlyMsg = "이메일/비밀번호 계정 생성이 현재 비활성화되어 있습니다.";
                                break;
                            default:
                                friendlyMsg = "회원가입 중 알 수 없는 오류가 발생했습니다. 잠시 후 다시 시도해주세요.";
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
