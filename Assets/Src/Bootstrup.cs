using System.Net.Http;
using Src.Auth;
using Src.Registry;
using UnityEngine;

public class Bootstrup : MonoBehaviour
{
    [SerializeField] private LogInView logInView;
    [SerializeField] private ServicesRegistry servicesRegistry;
    [SerializeField] private ServicesRegistryConfig servicesRegistryConfig;
    [SerializeField] private AuthOrchestrator authOrchestrator;
    
    private HttpClient _httpClient;
    private AuthService _authService;
    
    void Start()
    {
        _httpClient = new HttpClient();
        _authService = new AuthService(_httpClient);
        
        servicesRegistry.Init(servicesRegistryConfig, _httpClient);
        authOrchestrator.Init(_authService, servicesRegistry);
        logInView.Init(authOrchestrator);
    }
}
