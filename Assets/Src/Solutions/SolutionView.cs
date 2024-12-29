﻿using System;
using System.Collections.Generic;
using Src.Model;
using Src.Schemas;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Src.Solutions
{
    public class SolutionView : MonoBehaviour
    {
        private static Vector3 DEPOT_POSITION = Vector3.zero;
        
        [SerializeField] private Arrow arrowPrefab;
        [SerializeField] private Button solveButton;
        [SerializeField] private GameObject solutionLoadingScreen;
        [SerializeField] private GameObject solutionErrorScreen;
        [SerializeField] private GameObject validationWarning;
        [SerializeField] private TextMeshProUGUI validationWarningText;
        
        private readonly List<Arrow> _arrows = new();
        private WorkstationsBehaviourContainer _workstationsBehaviourContainer;
        private SchemaSaver _schemaSaver;
        private SolutionsOrchestrator _solutionsOrchestrator;
        private Schema _schema;
        private bool _schemaUpdated;
        
        public void Init(SolutionsOrchestrator solutionsOrchestrator, 
            SchemaSaver schemaSaver, WorkstationsBehaviourContainer workstationsBehaviourContainer)
        {
            _schemaSaver = schemaSaver;
            _solutionsOrchestrator = solutionsOrchestrator;
            _workstationsBehaviourContainer = workstationsBehaviourContainer;
        }

        private void Start()
        {
            solveButton.onClick.AddListener(OnSolveClicked);
        }
        
        public void SetSchema(Schema schema)
        {
            _schema = schema;
            _schemaUpdated = false;
        }

        public void SchemaUpdated()
        {
            ClearSolution();
            _schemaUpdated = true;
        }
        
        public void ClearSolution()
        {
            foreach (var arrow in _arrows)
            {
                Destroy(arrow.gameObject);
            }
            _arrows.Clear();
            _schemaUpdated = false;
        }

        private async void OnSolveClicked()
        {
            if (!IsSchemaValid())
            {
                return;
            }
            if (_schemaUpdated)
            {
                await _schemaSaver.SaveSchema(_schema);
                _schemaUpdated = false;
            }

            try
            {
                solutionLoadingScreen.SetActive(true);
                var solution = await _solutionsOrchestrator.GetSolution(_schema);
                solutionLoadingScreen.SetActive(false);
                ClearSolution();
                ShowSolution(solution);
            }
            catch (SolutionsServiceException e)
            {
                solutionLoadingScreen.SetActive(false);
                solutionErrorScreen.SetActive(true);
            }
        }

        private void ShowSolution(Dictionary<int, List<string>> solution)
        {
            foreach (var route in solution)
            {
                var routeColor = GetRandomColor();
                var workstationNames = route.Value;
                for (int i = 0; i < workstationNames.Count; i++)
                {
                    var fromPosition = DEPOT_POSITION;
                    if (i != 0)
                    {
                        fromPosition = _workstationsBehaviourContainer.GetBehaviourByWorkstationName(workstationNames[i - 1]).transform.position;
                    }
                    var toPosition = _workstationsBehaviourContainer.GetBehaviourByWorkstationName(workstationNames[i]).transform.position;
                    var arrow = Instantiate(arrowPrefab);
                    arrow.SetPositions(fromPosition, toPosition, 0.5f);
                    arrow.SetColor(routeColor);
                    _arrows.Add(arrow);
                }
                var lastWorkstation = _workstationsBehaviourContainer.GetBehaviourByWorkstationName(workstationNames[^1]);
                var depotPosition = DEPOT_POSITION;
                var lastWorkstationPosition = lastWorkstation.transform.position;
                var lastArrow = Instantiate(arrowPrefab);
                lastArrow.SetPositions(lastWorkstationPosition, depotPosition, 0.5f);
                lastArrow.SetColor(routeColor);
                _arrows.Add(lastArrow);
            }
        }

        private Color GetRandomColor()
        {
            //Return random bright color
            return Color.HSVToRGB(UnityEngine.Random.Range(0f, 1f), 1f, 1f);
        }

        private bool IsSchemaValid()
        {
            validationWarning.gameObject.SetActive(false);
            if (_schema == null)
            {
                validationWarningText.text = "Schema is not set";
                validationWarning.gameObject.SetActive(true);
                return false;
            }
            if (_schema.WorkStations.Count < 2)
            {
                validationWarningText.text = "\nSchema must have at least 2 workstations.\nOtherwise, why solving it with a specialized application?";
                validationWarning.gameObject.SetActive(true);

                return false;
            }
            if (WorkstationMaxDemandTooHigh())
            {
                return false;
            }
            if (WorkstationSumDemandTooHigh())
            {
                return false;
            }
            return true;
        }

        private bool WorkstationMaxDemandTooHigh()
        {
            var maxDemand = 0;
            foreach (var workstation in _schema.WorkStations)
            {
                if (workstation.Demand > maxDemand)
                {
                    maxDemand = workstation.Demand;
                }
            }

            if (maxDemand > _schema.AmrParameters.Capacity)
            {
                validationWarningText.text = "Schema cannot be solved!\nDemand of one or more workstations is higher than AMR capacity.\nPlease adjust the demand of workstations.";
                validationWarning.gameObject.SetActive(true);
                return true;
            }
            return false;
        }
        
        private bool WorkstationSumDemandTooHigh()
        {
            var sumDemand = 0;
            foreach (var workstation in _schema.WorkStations)
            {
                sumDemand += workstation.Demand;
            }

            if (sumDemand > _schema.AmrParameters.Capacity * _schema.AmrParameters.Quantity)
            {
                validationWarningText.text = "Schema cannot be solved!\nSum of workstations demand is higher than total AMR capacity.\nPlease adjust the demand of workstations.";
                validationWarning.gameObject.SetActive(true);
                return true;
            }
            return false;
        }
    }
}