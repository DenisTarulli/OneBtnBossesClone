using UnityEngine;
using Firebase;
using Firebase.Auth;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class AuthRegister : MonoBehaviour
{
    public DependencyStatus DependencyStatus;
    public FirebaseAuth Auth;
    public FirebaseUser User;

    [Header("Register")]
    [SerializeField] private TMP_InputField _usernameRegisterField;
    [SerializeField] private TMP_InputField _emailRegisterField;
    [SerializeField] private TMP_InputField _passwordRegisterField;
    [SerializeField] private TMP_InputField _passwordRegisterVerifyField;
    [SerializeField] private TMP_Text _warningRegisterText;
    
    private void Awake()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            DependencyStatus = task.Result;
            if (DependencyStatus == DependencyStatus.Available)
            {
                Auth = FirebaseAuth.DefaultInstance;
            }
            else
            {
                Debug.Log("no se pudo resolver las dependencia del firebase" + DependencyStatus);
            }
        });
    }
    public void RegisterButton()
    {
        StartCoroutine(Register(_emailRegisterField.text, _passwordRegisterField.text, _usernameRegisterField.text));
    }

    private IEnumerator Register(string _email, string _password, string _userName)
    {

        if (_passwordRegisterField.text != _passwordRegisterVerifyField.text)
        {
            _warningRegisterText.text = "Password Not Match";
            _warningRegisterText.color = Color.red;
        }
        else
        {
            var RegisterTask = Auth.CreateUserWithEmailAndPasswordAsync(_email, _password);

            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);


            if (RegisterTask.Exception != null)
            {
                Debug.Log(message: $"Failed To register task with{RegisterTask.Exception}");

                FirebaseException firebaseEX = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEX.ErrorCode;

                string message = "Register Failed:" + errorCode;

                _warningRegisterText.text = message;
                _warningRegisterText.color = Color.red;

            }
            else
            {

                Firebase.Auth.AuthResult result = RegisterTask.Result;

                User = result.User;

                if (User != null)
                {
                    UserProfile profile = new UserProfile { DisplayName = _userName };

                    var ProfileTask = User.UpdateUserProfileAsync(profile);

                    yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

                    if (ProfileTask.Exception != null)
                    {
                        Debug.Log(message: $"Failed To register task with{RegisterTask.Exception}");
                        FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                        _warningRegisterText.text = "Username Set Failed";
                        _warningRegisterText.color = Color.red;
                    }

                }
                Debug.LogFormat("User Register in succesfully: {0} ({1}", User.DisplayName, User.Email);
                _warningRegisterText.text = "Registration successful";
                _warningRegisterText.color = Color.green;

            }

        }
    }

    public void LoginScene()
    {
        SceneManager.LoadScene("Login");
    }   
}