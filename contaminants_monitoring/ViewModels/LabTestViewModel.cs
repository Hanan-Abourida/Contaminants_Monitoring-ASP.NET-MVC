using Contaminants_Monitoring.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Contaminants_Monitoring.ViewModels
{
    public class LabTestViewModel
    {
        public LabSample labSample { get; set; }
        public List<PesticideResidue> pesticideResidues { get; set; }
        public int selectedResID { get; set; }
        public List<Mycotoxin> mycotoxins { get; set; }
        public int selectedMycotoxinId { get; set; }
        public List<UOM> units { get; set; }
        public int? selectedUOM { get; set; }
        public bool caseClosed { get; set; }

        public string concentrationStatus { get; set; }


    }
    public class OneSampleTestsViewModel
    {
        public List<LabSample> labSamples { get; set; }
        public string identityCode { get; set; }

        public List<LabSample> filteredSamples { get; set; }
    }
}