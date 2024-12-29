using System;
using System.Collections.Generic;
using System.Net.Http;
using Src.Model;
using UnityEngine;

namespace Src.Solutions
{
    public class Test : MonoBehaviour
    {
        private async void Start()
        {
            HttpClient httpClient = new HttpClient();
            SolutionsService solutionsService = new SolutionsService(httpClient);
            solutionsService.SetUrl("http://localhost:8003");
            var workstation1 = new WorkStation
            {
                Name = "WS1",
                Demand = 10,
                DepotDistance = 10,
                X = 15,
                Y = 15
            };
            var workstation2 = new WorkStation
            {
                Name = "WS2",
                Demand = 20,
                DepotDistance = 20,
                X = 30,
                Y = 30
            };
            var schema = new Schema()
            {
                AmrParameters = new AmrParameters()
                {
                    Capacity = 100,
                    Quantity = 1
                },
                Id = 111,
                UserId = 1,
                WorkStations = new HashSet<WorkStation>
                {
                    workstation1,
                    workstation2
                },
                TransportationCosts = new HashSet<TransportationCost>
                {
                    new TransportationCost
                    {
                        FromStation = workstation1,
                        ToStation = workstation2,
                        Cost = 10
                    }
                }
            };
            var solution = await solutionsService.GetSolution(schema);
            foreach (var keyValuePair in solution)
            {
                Debug.Log("Key: " + keyValuePair.Key);
                foreach (var value in keyValuePair.Value)
                {
                    Debug.Log("Value: " + value);
                }
            }
        }
    }
}