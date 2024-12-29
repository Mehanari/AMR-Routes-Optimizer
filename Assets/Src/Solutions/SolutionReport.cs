using System.Collections.Generic;
using UnityEngine;

namespace Src.Solutions
{
    public class SolutionReport
    {
        public int TotalCost { get; set; }
        public float TotalEfficiency { get; set; }
        public List<AmrReport> AmrReports { get; } = new(); 
    }
    
    public class AmrReport
    {
        public int Cost { get; set; }
        public float Efficiency { get; set; }
        public string AmrName { get; set; }
        public Color RouteColor { get; set; }
    }
}