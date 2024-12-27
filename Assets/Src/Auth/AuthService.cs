using System.Net.Http;
using System.Threading.Tasks;

namespace Src.Auth
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private string _url;
        
        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        public void SetUrl(string url)
        {
            _url = url;
        }
        
        public async Task<string> GetToken(string username, string password)
        {
            var user = new User
            {
                Login = username,
                Password = password
            };
            var userJson = Newtonsoft.Json.JsonConvert.SerializeObject(user);
            var content = new StringContent(userJson, System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_url + "/auth", content);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var token = Newtonsoft.Json.JsonConvert.DeserializeObject<Token>(responseContent);
                return token.TokenStr;
            }
            else
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var json = Newtonsoft.Json.JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(responseContent);
                var message = json.Value<string>("detail");
                if (message == "Invalid credentials")
                {
                    throw new InvalidCredentialsException("Invalid credentials");
                }
            }
            throw new AuthServiceException("Failed to get token");
        }
        
        public async Task<int> GetUserId(string token)
        {
            var url = _url + "/exchange-token";
            var header = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            _httpClient.DefaultRequestHeaders.Authorization = header;
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var json = Newtonsoft.Json.JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(responseContent);
                return json.Value<int>("userId");
            }
            throw new AuthServiceException("Failed to get user id");
        }
    }
}