using Contaminants_Monitoring.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Contaminants_Monitoring.ViewModels
{
    public class LabViewModel
    {
        public LabSample labSample { get; set; }

        public List<SamplingType> samplingTypes { get; set; }
        public int selectedSamplingTypeId { get; set; }
        public bool received { get; set; }
        public string acceptedText { get; set; }
        public bool accepted { get; set; }
        public bool sentToTesting { get; set; }

        public ApplicationUser user { get; set; }
    }
}