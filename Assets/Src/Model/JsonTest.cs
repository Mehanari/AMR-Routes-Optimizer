using UnityEngine;

namespace Src.Model
{
    public class JsonTest : MonoBehaviour
    {
        private void Start()
        {
            WorkStation workStationA = new WorkStation
            {
                Name = "WS1",
                Demand = 10,
                DepotDistance = 5,
                X = 0,
                Y = 0
            };
            WorkStation workStationB = new WorkStation
            {
                Name = "WS2",
                Demand = 20,
                DepotDistance = 10,
                X = 10,
                Y = 0
            };
            TransportationCost transportationCost = new TransportationCost
            {
                FromStation = workStationA,
                ToStation = workStationB,
                Cost = 100
            };
            Schema schema = new Schema
            {
                UserId = 1,
                Id = 1,
                WorkStations = new System.Collections.Generic.HashSet<WorkStation>
                {
                    workStationA,
                    workStationB
                },
                TransportationCosts = new System.Collections.Generic.HashSet<TransportationCost>
                {
                    transportationCost
                },
                AmrParameters = new AmrParameters()
                {
                    Capacity = 1,
                    Quantity = 100
                }
            };
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(schema);
            Debug.Log(json);
        }
    }
}