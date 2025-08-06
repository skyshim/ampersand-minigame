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

        // SetActive ����
    }

    void HandleLogin()
    {
        string email = idInputField.text + "@dummy.com";
        string password = pwInputField.text;

        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogWarning("�α��� ����");
                toastMessage.ShowToast("�α��� ����", Color.red);
                return;
            }

            Debug.Log("�α��� ����!");
            toastMessage.ShowToast("�α��� ����!", Color.green);
        });
    }

    void HandleSignUp()
    {
        string email = idInputField.text + "@dummy.com";
        string password = pwInputField.text;

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogWarning("ȸ������ ����");
                toastMessage.ShowToast("ȸ������ ����", Color.red);
                return;
            }

            Debug.Log("ȸ������ ����!");
            toastMessage.ShowToast("ȸ������ ����!", Color.green);
        });
    }

    void ClosePopup()
    {
        gameObject.SetActive(false);
    }
}
