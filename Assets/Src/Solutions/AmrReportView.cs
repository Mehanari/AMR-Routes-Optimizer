using TMPro;
using UnityEngine;

namespace Src.Solutions
{
    public class AmrReportView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI amrNameText;
        [SerializeField] private TextMeshProUGUI costText;
        [SerializeField] private TextMeshProUGUI efficiencyText;
        private AmrReport _amrReport;
        
        public void Init(AmrReport amrReport)
        {
            _amrReport = amrReport;
            amrNameText.text = amrReport.AmrName;
            costText.text = amrReport.Cost.ToString();
            efficiencyText.text = $"{amrReport.Efficiency * 100:0.00}" + "%";
            amrNameText.color = amrReport.RouteColor;
        }
    }
}