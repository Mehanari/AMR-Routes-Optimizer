using System.Threading.Tasks;
using UnityEngine;

namespace Src.Auth
{
    public class TokenProvider : MonoBehaviour, ITokenProvider
    {
        private AuthOrchestrator _authOrchestrator;
        private string _username;
        private string _password;

        public void Init(AuthOrchestrator orchestrator)
        {
            _authOrchestrator = orchestrator;
        }

        public void SetCredentials(string username, string password)
        {
            _username = username;
            _password = password;
        }
        
        public async Task<string> GetToken()
        {
            return await _authOrchestrator.LogIn(_username, _password);
        } 
    }
}