using Contaminants_Monitoring.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Contaminants_Monitoring.ViewModels
{
    public class LabInventoryViewModel
    {
        public List<LabSample> labSamples { get; set; }
        //Add fields of search filter here
        public string status { get; set; }
        
    }
}