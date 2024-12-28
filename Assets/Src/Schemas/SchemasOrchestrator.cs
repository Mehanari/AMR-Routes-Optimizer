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
            await _servicesRegistry.AddSchemasServiceActivity();
            return schemas;
        }

        public async Task<Schema> CreateSchema()
        {
            var schemasServiceUrl = await _servicesRegistry.GetSchemasServiceUrl();
            _schemasService.SetUrl(schemasServiceUrl);
            var createdSchema = await _schemasService.CreateSchema();
            await _servicesRegistry.AddSchemasServiceActivity();
            return createdSchema;
        }
        
        public async Task<Schema> UpdateSchema(Schema schema)
        {
            var schemasServiceUrl = await _servicesRegistry.GetSchemasServiceUrl();
            _schemasService.SetUrl(schemasServiceUrl);
            var updatedSchema = await _schemasService.UpdateSchema(schema);
            await _servicesRegistry.AddSchemasServiceActivity();
            return updatedSchema;
        }
    }
}