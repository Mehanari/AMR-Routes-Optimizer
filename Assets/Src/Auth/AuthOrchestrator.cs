using System.Threading.Tasks;
using Src.Registry;
using UnityEngine;

namespace Src.Auth
{
    public class AuthOrchestrator : MonoBehaviour
    {
        private AuthService _authService;
        private ServicesRegistry _servicesRegistry;
        private string _token;

        public void Init(AuthService authService, ServicesRegistry servicesRegistry)
        {
            _authService = authService;
            _servicesRegistry = servicesRegistry;
        }
        
        public async Task<string> LogIn(string username, string password)
        {
            var authServiceUrl = await _servicesRegistry.GetAuthServiceUrl();
            _authService.SetUrl(authServiceUrl);
            _token = await _authService.GetToken(username, password);
            return _token;
        }
        
        public async Task<int> GetUserId()
        {
            var authServiceUrl = await _servicesRegistry.GetAuthServiceUrl();
            _authService.SetUrl(authServiceUrl);
            return await _authService.GetUserId(_token);
        }
        
        public string GetCachedToken()
        {
            return _token;
        }
    }
}