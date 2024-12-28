using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Src.Auth
{
    public class LogInView : MonoBehaviour
    {
        [SerializeField] private TMP_InputField usernameInput;
        [SerializeField] private TMP_InputField passwordInput;
        [SerializeField] private GameObject invalidCredentialsMessage;
        [SerializeField] private GameObject errorMessage;
        [SerializeField] private Button loginButton;
        [SerializeField] private UnityEvent onLoginSuccess = new();
        
        private AuthOrchestrator _authOrchestrator;
        private TokenProvider _tokenProvider;
        
        public void Init(AuthOrchestrator authOrchestrator, TokenProvider tokenProvider)
        {
            _authOrchestrator = authOrchestrator;
            _tokenProvider = tokenProvider;
        }
        
        private void Start()
        {
            loginButton.onClick.AddListener(OnLoginButtonClick);
        }

        private void OnDestroy()
        {
            loginButton.onClick.RemoveListener(OnLoginButtonClick);
        }

        private async void OnLoginButtonClick()
        {
            invalidCredentialsMessage.SetActive(false);
            var username = usernameInput.text;
            var password = passwordInput.text;
            try
            {
                await _authOrchestrator.LogIn(username, password);
                onLoginSuccess.Invoke();
                _tokenProvider.SetCredentials(username, password);
            }
            catch (InvalidCredentialsException e)
            {
                invalidCredentialsMessage.SetActive(true);
            }
            catch (AuthServiceException e)
            {
                errorMessage.SetActive(true);
            }
        }
    }
}