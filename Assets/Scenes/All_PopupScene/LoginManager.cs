using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using TMPro;

public class LoginManager : MonoBehaviour
{
    public InputField idInputField;
    public InputField pwInputField;
    public Button loginButton;
    public Button signupButton;
    public Button closeButton;

    public ToastMessage toastMessage;

    private FirebaseAuth auth;

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;

        loginButton.onClick.AddListener(HandleLogin);
        signupButton.onClick.AddListener(HandleSignUp);
        closeButton.onClick.AddListener(ClosePopup);

        // SetActive 제거
    }

    void HandleLogin()
    {
        string email = idInputField.text + "@dummy.com";
        string password = pwInputField.text;

        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogWarning("로그인 실패");
                toastMessage.ShowToast("로그인 실패", Color.red);
                return;
            }

            Debug.Log("로그인 성공!");
            toastMessage.ShowToast("로그인 성공!", Color.green);
        });
    }

    void HandleSignUp()
    {
        string email = idInputField.text + "@dummy.com";
        string password = pwInputField.text;

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogWarning("회원가입 실패");
                toastMessage.ShowToast("회원가입 실패", Color.red);
                return;
            }

            Debug.Log("회원가입 성공!");
            toastMessage.ShowToast("회원가입 성공!", Color.green);
        });
    }

    void ClosePopup()
    {
        gameObject.SetActive(false);
    }
}
