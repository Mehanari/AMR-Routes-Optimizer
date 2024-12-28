using System.Collections.Generic;
using Src.Model;
using Src.Solutions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Src.Schemas
{
    public class SchemasListView : MonoBehaviour
    {
        [SerializeField] private GameObject loadingScreen;
        [SerializeField] private SchemaListItem itemPrefab;
        [SerializeField] private Transform itemsContainer;
        [SerializeField] private Button createButton;
        [SerializeField] private UnityEvent<Schema> schemaSelected;
        [SerializeField] private UnityEvent<Schema> schemaCreated;
        
        private List<SchemaListItem> _items = new();
        private SchemasOrchestrator _schemasOrchestrator;
        private SolutionsOrchestrator _solutionsOrchestrator;

        private void Start()
        {
            createButton.onClick.AddListener(CreateSchema);
        }

        public void Init(SchemasOrchestrator schemasOrchestrator, SolutionsOrchestrator solutionsOrchestrator)
        {
            _schemasOrchestrator = schemasOrchestrator;
            _solutionsOrchestrator = solutionsOrchestrator;
        }
        
        private async void CreateSchema()
        {
            var schema = await _schemasOrchestrator.CreateSchema();
            var item = Instantiate(itemPrefab, itemsContainer);
            item.SetSchema(schema);
            item.SetName("Schema " + schema.Id);
            item.MarkAsUnsolved();
            item.Selected += SelectSchema;
            _items.Add(item);
            schemaCreated.Invoke(schema);
        }
        
        public async void LoadSchemas()
        {
            loadingScreen.SetActive(true);
            var schemas = await _schemasOrchestrator.GetAllSchemas();
            loadingScreen.SetActive(false);
            foreach (var item in _items)
            {
                item.Selected -= SelectSchema;
                Destroy(item.gameObject);
            }
            _items.Clear();
            foreach (var schema in schemas)
            {
                var item = Instantiate(itemPrefab, itemsContainer);
                item.SetSchema(schema);
                item.SetName("Schema " + schema.Id);
                var solved = await _solutionsOrchestrator.IsSchemaSolved(schema);
                if (solved)
                {
                    item.MarkAsSolved();
                }
                else
                {
                    item.MarkAsUnsolved();
                }
                item.Selected += SelectSchema;
                _items.Add(item);
            }
        }

        private void SelectSchema(Schema obj)
        {
            schemaSelected.Invoke(obj);
        }
    }
}