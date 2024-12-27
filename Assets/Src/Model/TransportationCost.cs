using System;
using Newtonsoft.Json;

namespace Src.Model
{
    [Serializable]
    public class TransportationCost
    {
        [JsonProperty("from_station")]
        public WorkStation FromStation { get; set; }
        [JsonProperty("to_station")]
        public WorkStation ToStation { get; set; }
        [JsonProperty("cost")]
        public int Cost { get; set; }
        
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            TransportationCost transportationCost = (TransportationCost) obj;
            return FromStation.Equals(transportationCost.FromStation) && ToStation.Equals(transportationCost.ToStation) && Cost == transportationCost.Cost;
        }
        
        public override int GetHashCode()
        {
            return FromStation.GetHashCode() + ToStation.GetHashCode() + Cost;
        }
    }
}