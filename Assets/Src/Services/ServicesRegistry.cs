using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

namespace Src.Services
{
    public class ServicesRegistry : MonoBehaviour
    {
        private ServicesRegistryConfig _config;
        private HttpClient _httpClient;
        private NameToVersion _authServiceNameToVersion;
        private NameToVersion _schemasServiceNameToVersion;
        private NameToVersion _solutionsServiceNameToVersion;
        
        public void Init(ServicesRegistryConfig config, HttpClient httpClient)
        {
            _config = config;
            _httpClient = httpClient;
        }

        public async Task<string> GetAuthServiceUrl()
        {
            foreach (var nameToVersion in _config.AuthServices)
            {
                if (await IsUp(nameToVersion.name, nameToVersion.version))
                {
                    _authServiceNameToVersion = nameToVersion;
                    return await GetUrl(nameToVersion.name, nameToVersion.version);
                }
            }
            throw new ServiceNotAvailableException("No auth service is available");
        }

        public async Task AddAuthServiceActivity()
        {
            await AddActivity(_authServiceNameToVersion.name, _authServiceNameToVersion.version);
        }
        
        public async Task<string> GetSchemasServiceUrl()
        {
            foreach (var nameToVersion in _config.SchemasServices)
            {
                if (await IsUp(nameToVersion.name, nameToVersion.version))
                {
                    _schemasServiceNameToVersion = nameToVersion;
                    return await GetUrl(nameToVersion.name, nameToVersion.version);
                }
            }
            throw new ServiceNotAvailableException("No schemas service is available");
        }
        
        public async Task AddSchemasServiceActivity()
        {
            await AddActivity(_schemasServiceNameToVersion.name, _schemasServiceNameToVersion.version);
        }
        
        public async Task<string> GetSolutionsServiceUrl()
        {
            foreach (var nameToVersion in _config.SolutionsServices)
            {
                if (await IsUp(nameToVersion.name, nameToVersion.version))
                {
                    _solutionsServiceNameToVersion = nameToVersion;
                    return await GetUrl(nameToVersion.name, nameToVersion.version);
                }
            }
            throw new ServiceNotAvailableException("No solutions service is available");
        }
        
        public async Task AddSolutionsServiceActivity()
        {
            await AddActivity(_solutionsServiceNameToVersion.name, _solutionsServiceNameToVersion.version);
        }
        
        private async Task AddActivity(string serviceName, string serviceVersion)
        {
            var url = $"{_config.RegistryUrl}/services/activity/{serviceName}/{serviceVersion}";
            var response = await _httpClient.PostAsync(url, null);
            if (!response.IsSuccessStatusCode)
            {
                throw new ServiceNotAvailableException($"Failed to add activity to service {serviceName} with version {serviceVersion}");
            }
        }

        private async Task<bool> IsUp(string serviceName, string serviceVersion)
        {
            var url = $"{_config.RegistryUrl}/services/status/{serviceName}/{serviceVersion}";
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var statusName = await response.Content.ReadAsStringAsync();
                Debug.Log($"Service {serviceName} with version {serviceVersion} is {statusName}");
                if (statusName == "\"UP\"")
                {
                    return true;
                }
            }
            return false;
        }
        
        private async Task<string> GetUrl(string serviceName, string serviceVersion)
        {
            var url = $"{_config.RegistryUrl}/services/address";
            var queryParameters = new Dictionary<string, string>
            {
                {"name", serviceName},
                {"version", serviceVersion}
            };
            var fullUrl = AddQueryParametersToUrl(url, queryParameters);
            var response = await _httpClient.GetAsync(fullUrl);
            if (response.IsSuccessStatusCode)
            {
                var serviceUrl = await response.Content.ReadAsStringAsync();
                Debug.Log($"Service {serviceName} with version {serviceVersion} is available at {serviceUrl}");
                return serviceUrl;
            }
            throw new ServiceNotAvailableException($"Service {serviceName} with version {serviceVersion} is not available");
        }

        private string AddQueryParametersToUrl(string url, Dictionary<string, string> queryParameters)
        {
            var queryString = string.Join("&", queryParameters.Select(kvp => $"{kvp.Key}={kvp.Value}"));
            return $"{url}?{queryString}";
        }
    }
}
