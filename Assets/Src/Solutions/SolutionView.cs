using System;
using System.Collections.Generic;
using System.Linq;
using Src.Model;
using Src.Schemas;
using TMPro;
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
        [SerializeField] private SolutionReportView solutionReportView;
        [SerializeField] private Button solutionReportOpenButton;
        
        private readonly List<Arrow> _arrows = new();
        private List<Color> _routeColors = new();
        private SolutionReport _solutionReport;
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
            solutionReportOpenButton.gameObject.SetActive(false);
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
            _routeColors.Clear();
            _solutionReport = null;
            _schemaUpdated = false;
            solutionReportView.Clear();
            solutionReportView.gameObject.SetActive(false);
            solutionReportOpenButton.gameObject.SetActive(false);
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
                GenerateReport(solution);
            }
            catch (SolutionsServiceException e)
            {
                solutionLoadingScreen.SetActive(false);
                solutionErrorScreen.SetActive(true);
            }
        }

        private void GenerateReport(Dictionary<int, List<string>> solution)
        {
            var report = new SolutionReport();
            var totalDemand = 0;
            foreach (var workstation in _schema.WorkStations)
            {
                totalDemand += workstation.Demand;
            }
            var amrsUsed = solution.Count;
            var capacityUsed = amrsUsed * _schema.AmrParameters.Capacity;
            var totalEfficiency = (float)totalDemand / capacityUsed;
            var totalCost = 0;
            var routesCosts = new List<int>();
            foreach (var route in solution)
            {
                var routeCost = 0;
                var workstationNames = route.Value;
                for (int i = 0; i < workstationNames.Count; i++)
                {
                    var currentStation = _schema.WorkStations.First(w => w.Name == workstationNames[i]);
                    if (i == 0)
                    {
                        routeCost += currentStation.DepotDistance;
                        continue;
                    }
                    if (i == workstationNames.Count - 1)
                    {
                        routeCost += currentStation.DepotDistance;
                        continue;
                    }
                    var nextStation = _schema.WorkStations.First(w => w.Name == workstationNames[i + 1]);
                    var transportationCost = _schema.TransportationCosts.First(t =>
                        (t.FromStation.Name == currentStation.Name && t.ToStation.Name == nextStation.Name) ||
                        (t.ToStation.Name == currentStation.Name && t.FromStation.Name == nextStation.Name));
                    routeCost += transportationCost.Cost;
                }
                routesCosts.Add(routeCost);
                totalCost += routeCost;
            }
            var routesDemands = new List<int>();
            foreach (var route in solution)
            {
                var routeDemand = 0;
                foreach (var workstationName in route.Value)
                {
                    var workstation = _schema.WorkStations.First(w => w.Name == workstationName);
                    routeDemand += workstation.Demand;
                }
                routesDemands.Add(routeDemand);
            }
            var routesEfficiencies = new List<float>();
            foreach (var demand in routesDemands)
            {
                routesEfficiencies.Add((float)demand / _schema.AmrParameters.Capacity);
            }
            
            report.TotalCost = totalCost;
            report.TotalEfficiency = totalEfficiency;
            for (int i = 0; i < solution.Count; i++)
            {
                var amrReport = new AmrReport();
                amrReport.Cost = routesCosts[i];
                amrReport.Efficiency = routesEfficiencies[i];
                amrReport.AmrName = $"AMR {i + 1}";
                amrReport.RouteColor = _routeColors[i];
                report.AmrReports.Add(amrReport);
            }
            _solutionReport = report;
            solutionReportView.SetSolutionReport(report);
            solutionReportOpenButton.gameObject.SetActive(true);
        }

        private void ShowSolution(Dictionary<int, List<string>> solution)
        {
            foreach (var route in solution)
            {
                var routeColor = GetRandomColor();
                _routeColors.Add(routeColor);
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