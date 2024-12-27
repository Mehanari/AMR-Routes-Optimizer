using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Src.Model
{
    [Serializable]
    public class Schema
    {
        [JsonProperty("user_id")]
        public int UserId { get; set; }
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("workstations")]
        public HashSet<WorkStation> WorkStations { get; set; }
        [JsonProperty("transportation_costs")]
        public HashSet<TransportationCost> TransportationCosts { get; set; }
        [JsonProperty("amr_parameters")]
        public AmrParameters AmrParameters { get; set; }
    }
}