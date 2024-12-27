using System.Net.Http;
using Src.Services;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] ServicesRegistryConfig _config;
    [SerializeField] ServicesRegistry _servicesRegistry;
    
    async void Start()
    {
        _servicesRegistry.Init(_config, new HttpClient());
        var authServiceUrl = await _servicesRegistry.GetAuthServiceUrl();
        Debug.Log(authServiceUrl);
    }
}