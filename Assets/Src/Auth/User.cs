using System;
using Newtonsoft.Json;

namespace Src.Auth
{
    [Serializable]
    public class User
    {
        [JsonProperty("login")]
        public string Login { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}