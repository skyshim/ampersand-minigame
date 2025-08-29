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

    public event Action OnLoginSuccess;

    async void Awake()
    {
        // Firebase 의존성 체크
        var dependencyTask = await FirebaseApp.CheckAndFixDependenciesAsync();
        if (dependencyTask == DependencyStatus.Available)
        {
            auth = FirebaseAuth.DefaultInstance;
            firebaseReady = true;
            Debug.Log("Firebase 초기화 완료");
        }
        else
        {
            Debug.LogError("Firebase 초기화 실패: " + dependencyTask);
            toastMessage.ShowToast("Firebase 초기화 실패", Color.red);
        }
    }

    void Start()
    {
        loginButton.onClick.AddListener(HandleLogin);
        signupButton.onClick.AddListener(HandleSignUp);
        closeButton.onClick.AddListener(ClosePopup);
    }

    async void HandleLogin()
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

        try
        {
            var userCredential = await auth.SignInWithEmailAndPasswordAsync(email, password);
            Debug.Log("로그인 성공!");

            toastMessage.ShowToast("로그인 성공!", Color.green);
            StartCoroutine(ClosePopupDelayed(2f));
            OnLoginSuccess?.Invoke();
        }
        catch (FirebaseException fe)
        {
            string friendlyMsg = fe.ErrorCode switch
            {
                (int)AuthError.InvalidEmail => "아이디 형식이 잘못되었어요. 올바른 아이디를 입력해주세요.",
                (int)AuthError.WrongPassword => "비밀번호가 틀렸습니다. 다시 확인해주세요.",
                (int)AuthError.UserNotFound => "존재하지 않는 계정이에요. 회원가입 먼저 해주세요.",
                (int)AuthError.EmailAlreadyInUse => "이미 가입된 계정입니다. 다른 아이디를 사용해주세요.",
                (int)AuthError.WeakPassword => "비밀번호가 너무 약해요. 6자 이상으로 입력해주세요.",
                _ => "로그인 중 알 수 없는 오류가 발생했습니다. 잠시 후 다시 시도해주세요."
            };

            Debug.LogError("로그인 실패: " + friendlyMsg);
            toastMessage.ShowToast(friendlyMsg, Color.red);
        }
        catch (Exception e)
        {
            Debug.LogError("로그인 예외: " + e.Message);
            toastMessage.ShowToast("알 수 없는 오류가 발생했습니다.", Color.red);
        }
    }

    private IEnumerator ClosePopupDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);
        ClosePopup();
    }

    async void HandleSignUp()
    {
        if (!firebaseReady)
        {
            toastMessage.ShowToast("Firebase 초기화 중...", Color.yellow);
            return;
        }

        string email = idInputField.text + "@dummy.com";
        string password = pwInputField.text;

        try
        {
            var userCredential = await auth.CreateUserWithEmailAndPasswordAsync(email, password);
            Debug.Log("회원가입 성공!");
            toastMessage.ShowToast("회원가입 성공!", Color.green);

            UserProfile profile = new UserProfile { DisplayName = idInputField.text };
            await userCredential.User.UpdateUserProfileAsync(profile);
        }
        catch (FirebaseException fe)
        {
            string friendlyMsg = fe.ErrorCode switch
            {
                (int)AuthError.EmailAlreadyInUse => "이미 가입된 계정이에요. 다른 아이디를 사용해주세요.",
                (int)AuthError.InvalidEmail => "아이디 형식이 잘못되었어요. 올바른 아이디를 입력해주세요.",
                (int)AuthError.WeakPassword => "비밀번호가 너무 약해요. 6자 이상으로 입력해주세요.",
                (int)AuthError.OperationNotAllowed => "이메일/비밀번호 계정 생성이 현재 비활성화되어 있습니다.",
                _ => "회원가입 중 알 수 없는 오류가 발생했습니다. 잠시 후 다시 시도해주세요."
            };

            Debug.LogError("회원가입 실패: " + friendlyMsg);
            toastMessage.ShowToast(friendlyMsg, Color.red);
        }
        catch (Exception e)
        {
            Debug.LogError("회원가입 예외: " + e.Message);
            toastMessage.ShowToast("알 수 없는 오류가 발생했습니다.", Color.red);
        }
    }

    void ClosePopup()
    {
        loginPopup.SetActive(false);
    }
}
