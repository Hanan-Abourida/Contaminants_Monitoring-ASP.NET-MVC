using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Contaminants_Monitoring.ViewModels
{
    public class SamplesCounter
    {
        public string commodity { get; set; }
        public int OnePROnly { get; set;}
        public double OnePROnly_percent { get; set; }
        public int twoPROnly { get; set; }
        public double twoPROnly_percent { get; set; }
        public int threeToTenPR{ get; set; }
        public double threeToTenPR_percent { get; set; }
        public int TenToMaxPR { get; set; }
        public double TenToMaxPR_percent { get; set; }
    }
}