using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Src.Model;
using UnityEngine;

namespace Src.Schemas
{
    public class SchemasService
    {
        private readonly ITokenProvider _tokenProvider;
        private readonly HttpClient _httpClient;
        private string _url;
        
        public SchemasService(ITokenProvider tokenProvider, HttpClient httpClient)
        {
            _tokenProvider = tokenProvider;
            _httpClient = httpClient;
        }
        
        public void SetUrl(string url)
        {
            _url = url;
        }

        public async Task<Schema> CreateSchema()
        {
            await SetAuthHeader();
            var url = _url + "/schemas/";
            var response = await _httpClient.PostAsync(url, null);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var schema = Newtonsoft.Json.JsonConvert.DeserializeObject<Schema>(responseContent);
                return schema;
            }
            throw new SchemasServiceException("Failed to create schema");
        }
        
        public async Task<Schema> UpdateSchema(Schema schema)
        {
            await SetAuthHeader();
            var url = _url + "/schemas/";
            var schemaJson = Newtonsoft.Json.JsonConvert.SerializeObject(schema);
            var content = new StringContent(schemaJson, System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var updatedSchema = Newtonsoft.Json.JsonConvert.DeserializeObject<Schema>(responseContent);
                return updatedSchema;
            }
            throw new SchemasServiceException("Failed to update schema");
        }

        public async Task<List<Schema>> GetAllSchemas()
        {
            await SetAuthHeader();
            var url = _url + "/schemas/";
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var schemas = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Schema>>(responseContent);
                foreach (var schema in schemas)
                {
                    Debug.Log("Retrieved schema with id: " + schema.Id);
                }
                return schemas;
            }
            throw new SchemasServiceException("Failed to get schemas");
        }
        
        public async Task<Schema> GetSchema(string id)
        {
            await SetAuthHeader();
            var url = _url + "/schemas/" + id;
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var schema = Newtonsoft.Json.JsonConvert.DeserializeObject<Schema>(responseContent);
                return schema;
            }
            throw new SchemasServiceException("Failed to get schema");
        }

        private async Task SetAuthHeader()
        {
            var token = await _tokenProvider.GetToken();
            var header = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            _httpClient.DefaultRequestHeaders.Authorization = header; 
        }
    }
}