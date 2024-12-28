using System.Threading.Tasks;
using Src.Registry;
using UnityEngine;

namespace Src.Auth
{
    public class AuthOrchestrator : MonoBehaviour
    {
        private AuthService _authService;
        private ServicesRegistry _servicesRegistry;

        public void Init(AuthService authService, ServicesRegistry servicesRegistry)
        {
            _authService = authService;
            _servicesRegistry = servicesRegistry;
        }
        
        public async Task<string> LogIn(string username, string password)
        {
            var authServiceUrl = await _servicesRegistry.GetAuthServiceUrl();
            _authService.SetUrl(authServiceUrl);
            var token = await _authService.GetToken(username, password);
            await _servicesRegistry.AddAuthServiceActivity();
            return token;
        }
    }
}