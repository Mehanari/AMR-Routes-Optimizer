using System;
using System.Collections.Generic;
using System.Linq;
using Src.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Src.Schemas
{
    public class SchemaEditor : MonoBehaviour
    {
        [SerializeField] private Button addWorkstationButton;
        [SerializeField] private Button saveButton;
        //[SerializeField] private TMP_InputField amrQuantityInput;
        //SerializeField] private TMP_InputField amrCapacityInput;
        [SerializeField] private WorkstationEditor workstationEditor;
        [SerializeField] private WorkstationBehaviour workstationPrefab;
        [SerializeField] private GameObject serviceErrorMessage;
        
        private readonly List<WorkstationBehaviour> _workstationBehaviours = new();
        private SchemasOrchestrator _schemasOrchestrator;
        private Schema _schema;
        
        public void Init(SchemasOrchestrator schemasOrchestrator)
        {
            _schemasOrchestrator = schemasOrchestrator;
        }

        private void Start()
        {
            addWorkstationButton.onClick.AddListener(OnAddWorkstationButtonClick);
            saveButton.onClick.AddListener(OnSaveButtonClick);
            workstationEditor.DeleteClicked += OnWorkstationDelete;
        }

        private void OnWorkstationDelete((WorkStation workstation, WorkstationBehaviour behaviour) obj)
        {
            _schema.WorkStations.Remove(obj.workstation);
            _workstationBehaviours.Remove(obj.behaviour);
            Destroy(obj.behaviour.gameObject);
        }

        private async void OnSaveButtonClick()
        {
            try
            {
                await _schemasOrchestrator.UpdateSchema(_schema);
            }
            catch (SchemasServiceException e)
            {
                serviceErrorMessage.SetActive(true);
            }
        }

        private void OnAddWorkstationButtonClick()
        {
            var workStation = new WorkStation()
            {
                Name = GetFreeWorkstationName(),
                Demand = 0,
                X = 0,
                Y = 200
            };
            _schema.WorkStations.Add(workStation);
            var workstationBehaviour = Instantiate(workstationPrefab, transform);
            workstationBehaviour.Init(workStation, workstationEditor);
            _workstationBehaviours.Add(workstationBehaviour);
        }

        private string GetFreeWorkstationName()
        {
            var name = "WK" + _schema.WorkStations.Count;
            var i = 0;
            while (_schema.WorkStations.Any(ws => ws.Name == name))
            {
                i++;
                name = "WK" + _schema.WorkStations.Count + i;
            }

            return name;
        }

        public void LoadSchema(Schema schema)
        {
            _schema = schema;
            workstationEditor.SetSchema(schema);
            foreach (var workStation in _schema.WorkStations)
            {
                var workstationBehaviour = Instantiate(workstationPrefab, transform);
                workstationBehaviour.Init(workStation, workstationEditor);
                _workstationBehaviours.Add(workstationBehaviour);
            }
            //amrCapacityInput.text = _schema.AmrParameters.Capacity.ToString();
            //amrQuantityInput.text = _schema.AmrParameters.Quantity.ToString();
        }

        public void Clear()
        {
            foreach (var workstationBehaviour in _workstationBehaviours)
            {
                Destroy(workstationBehaviour.gameObject);
            }
            _workstationBehaviours.Clear();
            _schema = null;
            serviceErrorMessage.SetActive(false);
        }
    }
}