using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Src.Solutions
{
    public class SolutionReportView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI totalCostText;
        [SerializeField] private TextMeshProUGUI totalEfficiencyText;
        [SerializeField] private Transform amrReportsContainer;
        [SerializeField] private AmrReportView amrReportPrefab;
        private List<AmrReportView> _amrReportViews = new();
        private SolutionReport _solutionReport;
        
        public void SetSolutionReport(SolutionReport solutionReport)
        {
            Clear();
            _solutionReport = solutionReport;
            totalCostText.text = solutionReport.TotalCost.ToString();
            totalEfficiencyText.text = $"{solutionReport.TotalEfficiency * 100:0.00}" + "%";
            foreach (var amrReport in solutionReport.AmrReports)
            {
                var amrReportView = Instantiate(amrReportPrefab, amrReportsContainer);
                amrReportView.Init(amrReport);
                _amrReportViews.Add(amrReportView);
            }
        }
        
        public void Clear()
        {
            totalCostText.text = "";
            totalEfficiencyText.text = "";
            foreach (var amrReportView in _amrReportViews)
            {
                Destroy(amrReportView.gameObject);
            }
            _amrReportViews.Clear();
        }
    }
}