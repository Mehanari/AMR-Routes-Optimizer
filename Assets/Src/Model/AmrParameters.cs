using System;
using Newtonsoft.Json;

namespace Src.Model
{
    [Serializable]
    public class AmrParameters
    {
        [JsonProperty("quantity")]
        public int Quantity { get; set; }
        [JsonProperty("capacity")]
        public int Capacity { get; set; }
    }
}
