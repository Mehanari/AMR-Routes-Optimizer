using System;
using Src.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Src.Schemas
{
    public class SchemaListItem : MonoBehaviour
    {
        [SerializeField] private Button selectButton;
        [SerializeField] private TextMeshProUGUI schemaNameText;
        [SerializeField] private GameObject solvedStatusMark;
        [SerializeField] private GameObject unsolvedStatusMark;
        private Schema _schema;

        public event Action<Schema> Selected;
        
        private void Start()
        {
            selectButton.onClick.AddListener(OnSelectButtonClick);
        }

        private void OnSelectButtonClick()
        {
            Selected?.Invoke(_schema);
        }

        public void SetSchema(Schema schema)
        {
            _schema = schema;
        }
        
        public void SetName(string name)
        {
            schemaNameText.text = name;
        }
        
        public void MarkAsSolved()
        {
            solvedStatusMark.SetActive(true);
            unsolvedStatusMark.SetActive(false);
        }
        
        public void MarkAsUnsolved()
        {
            solvedStatusMark.SetActive(false);
            unsolvedStatusMark.SetActive(true);
        }
    }
}