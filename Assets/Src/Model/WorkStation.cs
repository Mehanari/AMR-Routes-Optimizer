using System;
using Newtonsoft.Json;

namespace Src.Model
{
    [Serializable]
    public class WorkStation
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("demand")]
        public int Demand { get; set; }
        [JsonProperty("depot_distance")]
        public int DepotDistance { get; set; }
        [JsonProperty("x")]
        public int X { get; set; }
        [JsonProperty("y")]
        public int Y { get; set; }
        
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            WorkStation workStation = (WorkStation) obj;
            return Name.Equals(workStation.Name) && Demand == workStation.Demand && DepotDistance == workStation.DepotDistance && X == workStation.X && Y == workStation.Y;
        }
        
        public override int GetHashCode()
        {
            return Name.GetHashCode() + Demand + DepotDistance + X + Y;
        }
    }
}