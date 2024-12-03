using UnityEngine;
using Firebase;
using Firebase.Auth;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class AuthLogin : MonoBehaviour
{
    public DependencyStatus dependencyStatus;
    private FirebaseAuth auth;
    public FirebaseUser user;

    [SerializeField] private float _nextSceneDelay;

    [Header("Login")]
    [SerializeField] private TMP_InputField _emailLoginField;
    [SerializeField] private TMP_InputField _passwordLoginField;
    [SerializeField] private TMP_Text _warningLoginText;


    private void Awake()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
            }
            else
            {
                Debug.Log("could not resolve firebase dependency" + dependencyStatus);
            }
        });
    }

    public void LoginButton()
    {
        StartCoroutine(Login(_emailLoginField.text, _passwordLoginField.text));
    }

    private IEnumerator Login(string _email, string _password)
    {

        var LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);

        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);


        if (LoginTask.Exception != null)
        {
            Debug.Log(message: $"Failed To register task with{LoginTask.Exception}");
            FirebaseException firebaseEX = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEX.ErrorCode;

            string message = "Login Failed:" + errorCode;
            _warningLoginText.text = message;
            _warningLoginText.color = Color.red;
        }
        else
        {
            Firebase.Auth.AuthResult result = LoginTask.Result;

            user = result.User;
            Debug.LogFormat("Firebase user sign-in successfully: {0} ({1})", user.DisplayName, user.Email);
            _warningLoginText.text = "Login successful";
            _warningLoginText.color = Color.green;

            StartCoroutine(NextScene());
        }
    }

    public void RegisterScene()
    {
        SceneManager.LoadScene("Register");
    }

    private IEnumerator NextScene()
    {
        yield return new WaitForSeconds(_nextSceneDelay);

        SceneManager.LoadScene("LevelSelector");
    }
}