using System;
using Newtonsoft.Json;

namespace Src.Auth
{
    [Serializable]
    public class Token
    {
        [JsonProperty("token")]
        public string TokenStr { get; set; }
    }
}