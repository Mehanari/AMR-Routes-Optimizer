using System.Collections.Generic;
using System.Threading.Tasks;
using Src.Model;
using Src.Registry;
using UnityEngine;

namespace Src.Schemas
{
    public class SchemasOrchestrator : MonoBehaviour
    {
        private ServicesRegistry _servicesRegistry;
        private SchemasService _schemasService;
        
        public void Init(ServicesRegistry servicesRegistry, SchemasService schemasService)
        {
            _servicesRegistry = servicesRegistry;
            _schemasService = schemasService;
        }

        public async Task<List<Schema>> GetAllSchemas()
        {
            var schemasServiceUrl = await _servicesRegistry.GetSchemasServiceUrl();
            _schemasService.SetUrl(schemasServiceUrl);
            var schemas = await _schemasService.GetAllSchemas();
            return schemas;
        }

        public async Task<Schema> CreateSchema()
        {
            var schemasServiceUrl = await _servicesRegistry.GetSchemasServiceUrl();
            _schemasService.SetUrl(schemasServiceUrl);
            var createdSchema = await _schemasService.CreateSchema();
            return createdSchema;
        }
    }
}