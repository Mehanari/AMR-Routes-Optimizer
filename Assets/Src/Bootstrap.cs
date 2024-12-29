using System.Net.Http;
using Src.Auth;
using Src.Registry;
using Src.Schemas;
using Src.Solutions;
using UnityEngine;

namespace Src
{
    public class Bootstrap : MonoBehaviour
    {
        [Header("Authentication")]
        [SerializeField] private LogInView logInView;
        [SerializeField] private AuthOrchestrator authOrchestrator;
        [SerializeField] private TokenProvider tokenProvider;
        private AuthService _authService;

        [Header("Schemas")]
        [SerializeField] private SchemasListView listView;
        [SerializeField] private SchemaEditor schemaEditor;
        [SerializeField] private WorkstationEditor workstationEditor;
        [SerializeField] private SchemasOrchestrator schemasOrchestrator;
        [SerializeField] private SchemaSaver schemaSaver;
        [SerializeField] private WorkstationsBehaviourContainer workstationsBehaviourContainer;
        private SchemasService _schemasService;
        
        [Header("Solutions")]
        [SerializeField] private SolutionsOrchestrator solutionsOrchestrator;
        [SerializeField] private SolutionView solutionView;
        private SolutionsService _solutionsService;
        
        [Header("Services")]
        [SerializeField] private ServicesRegistryConfig servicesRegistryConfig;
        [SerializeField] private ServicesRegistry servicesRegistry;
        
        
        private HttpClient _httpClient;
    
        void Start()
        {
            _httpClient = new HttpClient();
            _authService = new AuthService(_httpClient);
            _schemasService = new SchemasService(tokenProvider, _httpClient);
            _solutionsService = new SolutionsService(_httpClient);
            
            servicesRegistry.Init(servicesRegistryConfig, _httpClient);
            authOrchestrator.Init(_authService, servicesRegistry);
            schemasOrchestrator.Init(servicesRegistry, _schemasService);
            solutionsOrchestrator.Init(_solutionsService, servicesRegistry);
            tokenProvider.Init(authOrchestrator);
            logInView.Init(authOrchestrator, tokenProvider);
            listView.Init(schemasOrchestrator, solutionsOrchestrator);
            schemaSaver.Init(schemasOrchestrator);
            schemaEditor.Init(workstationsBehaviourContainer, schemaSaver);
            solutionView.Init(solutionsOrchestrator, schemaSaver, workstationsBehaviourContainer);
        }
    }
}
