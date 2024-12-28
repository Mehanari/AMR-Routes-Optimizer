using System.Threading.Tasks;
using Src.Model;
using Src.Registry;
using UnityEngine;

namespace Src.Solutions
{
    public class SolutionsOrchestrator : MonoBehaviour
    {
        private ServicesRegistry _servicesRegistry;
        private SolutionsService _solutionsService;
        
        public void Init(SolutionsService solutionsService, ServicesRegistry servicesRegistry)
        {
            _solutionsService = solutionsService;
            _servicesRegistry = servicesRegistry;
        }

        public async Task<bool> IsSchemaSolved(Schema schema)
        {
            var solutionsServiceUrl = await _servicesRegistry.GetSolutionsServiceUrl();
            _solutionsService.SetUrl(solutionsServiceUrl);
            var isSolved = await _solutionsService.IsSchemaSolved(schema.Id);
            return isSolved;
        }
    }
}