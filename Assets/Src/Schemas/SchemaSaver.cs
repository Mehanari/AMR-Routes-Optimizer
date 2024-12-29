using System.Threading.Tasks;
using Src.Model;
using UnityEngine;

namespace Src.Schemas
{
    public class SchemaSaver : MonoBehaviour
    {
        [SerializeField] private GameObject savingLoadingScreen;
        [SerializeField] private GameObject savingErrorScreen;
        
        private SchemasOrchestrator _schemasOrchestrator;

        public void Init(SchemasOrchestrator schemasOrchestrator)
        {
            _schemasOrchestrator = schemasOrchestrator;
        }
        
        public async Task SaveSchema(Schema schema)
        {
            try
            {
                savingLoadingScreen.SetActive(true);
                await _schemasOrchestrator.UpdateSchema(schema);
                savingLoadingScreen.SetActive(false);
            }
            catch (SchemasServiceException e)
            {
                savingLoadingScreen.SetActive(false);
                savingErrorScreen.SetActive(true);
            }
        }
    }
}