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
        // Firebase ������ üũ
        var dependencyTask = await FirebaseApp.CheckAndFixDependenciesAsync();
        if (dependencyTask == DependencyStatus.Available)
        {
            auth = FirebaseAuth.DefaultInstance;
            firebaseReady = true;
            Debug.Log("Firebase �ʱ�ȭ �Ϸ�");
        }
        else
        {
            Debug.LogError("Firebase �ʱ�ȭ ����: " + dependencyTask);
            toastMessage.ShowToast("Firebase �ʱ�ȭ ����", Color.red);
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

        try
        {
            var userCredential = await auth.SignInWithEmailAndPasswordAsync(email, password);
            Debug.Log("�α��� ����!");

            toastMessage.ShowToast("�α��� ����!", Color.green);
            StartCoroutine(ClosePopupDelayed(2f));
            OnLoginSuccess?.Invoke();
        }
        catch (FirebaseException fe)
        {
            string friendlyMsg = fe.ErrorCode switch
            {
                (int)AuthError.InvalidEmail => "���̵� ������ �߸��Ǿ����. �ùٸ� ���̵� �Է����ּ���.",
                (int)AuthError.WrongPassword => "��й�ȣ�� Ʋ�Ƚ��ϴ�. �ٽ� Ȯ�����ּ���.",
                (int)AuthError.UserNotFound => "�������� �ʴ� �����̿���. ȸ������ ���� ���ּ���.",
                (int)AuthError.EmailAlreadyInUse => "�̹� ���Ե� �����Դϴ�. �ٸ� ���̵� ������ּ���.",
                (int)AuthError.WeakPassword => "��й�ȣ�� �ʹ� ���ؿ�. 6�� �̻����� �Է����ּ���.",
                _ => "�α��� �� �� �� ���� ������ �߻��߽��ϴ�. ��� �� �ٽ� �õ����ּ���."
            };

            Debug.LogError("�α��� ����: " + friendlyMsg);
            toastMessage.ShowToast(friendlyMsg, Color.red);
        }
        catch (Exception e)
        {
            Debug.LogError("�α��� ����: " + e.Message);
            toastMessage.ShowToast("�� �� ���� ������ �߻��߽��ϴ�.", Color.red);
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
            toastMessage.ShowToast("Firebase �ʱ�ȭ ��...", Color.yellow);
            return;
        }

        string email = idInputField.text + "@dummy.com";
        string password = pwInputField.text;

        try
        {
            var userCredential = await auth.CreateUserWithEmailAndPasswordAsync(email, password);
            Debug.Log("ȸ������ ����!");
            toastMessage.ShowToast("ȸ������ ����!", Color.green);

            UserProfile profile = new UserProfile { DisplayName = idInputField.text };
            await userCredential.User.UpdateUserProfileAsync(profile);
        }
        catch (FirebaseException fe)
        {
            string friendlyMsg = fe.ErrorCode switch
            {
                (int)AuthError.EmailAlreadyInUse => "�̹� ���Ե� �����̿���. �ٸ� ���̵� ������ּ���.",
                (int)AuthError.InvalidEmail => "���̵� ������ �߸��Ǿ����. �ùٸ� ���̵� �Է����ּ���.",
                (int)AuthError.WeakPassword => "��й�ȣ�� �ʹ� ���ؿ�. 6�� �̻����� �Է����ּ���.",
                (int)AuthError.OperationNotAllowed => "�̸���/��й�ȣ ���� ������ ���� ��Ȱ��ȭ�Ǿ� �ֽ��ϴ�.",
                _ => "ȸ������ �� �� �� ���� ������ �߻��߽��ϴ�. ��� �� �ٽ� �õ����ּ���."
            };

            Debug.LogError("ȸ������ ����: " + friendlyMsg);
            toastMessage.ShowToast(friendlyMsg, Color.red);
        }
        catch (Exception e)
        {
            Debug.LogError("ȸ������ ����: " + e.Message);
            toastMessage.ShowToast("�� �� ���� ������ �߻��߽��ϴ�.", Color.red);
        }
    }

    void ClosePopup()
    {
        loginPopup.SetActive(false);
    }
}
