using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Src.Model;
using UnityEngine;

namespace Src.Solutions
{
    public class SolutionsService
    {
        private readonly HttpClient _httpClient;
        private string _url;
        
        public SolutionsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        public void SetUrl(string url)
        {
            _url = url;
        }
        
        public async Task<bool> IsSchemaSolved(int schemaId)
        {
            var url = _url + "/has_actual_solution/" + schemaId;
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var json = Newtonsoft.Json.JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(responseContent);
                Debug.Log("IsSchemaSolved json: " + json);
                var isSolved = json.Value<bool>("has_actual_solution");
                Debug.Log("IsSchemaSolved isSolved: " + isSolved);
                return isSolved;
            }
            throw new SolutionsServiceException("Failed to check if schema is solved");
        }

        public async Task<Dictionary<int, List<string>>> GetSolution(Schema schema)
        {
            var url = _url + "/solve";
            var schemaJson = Newtonsoft.Json.JsonConvert.SerializeObject(schema);
            var content = new StringContent(schemaJson, System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var json = Newtonsoft.Json.JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(responseContent);
                Debug.Log("GetSolution json: " + json);
                var solutionJson = json.Value<Newtonsoft.Json.Linq.JObject>("solution");
                var solution = solutionJson.ToObject<Dictionary<int, List<string>>>();
                Debug.Log("GetSolution solution: " + solution);
                return solution;
            }
            throw new SolutionsServiceException("Failed to get solution");
        }
    }
}