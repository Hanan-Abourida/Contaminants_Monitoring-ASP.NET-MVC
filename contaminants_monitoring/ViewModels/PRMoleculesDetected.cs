using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Contaminants_Monitoring.ViewModels
{
    public class PRMoleculesDetected
    {
        public int molecules_1sample { get; set; }
        public double molecules_1sample_percent { get; set; }
        public int molecules_2to10sample { get; set; }
        public double molecules_2to10sample_percent { get; set; }
        public int molecules_11to50sample { get; set; }
        public double molecules_11to50sample_percent { get; set; }
        public int molecules_51to100sample { get; set; }
        public double molecules_51to100sample_percent { get; set; }
        public int molecules_101to1000sample { get; set; }
        public double molecules_101to1000sample_percent { get; set; }
        public int molecules_more1000sample { get; set; }
        public double molecules_more1000sample_percent { get; set; }

    }
}