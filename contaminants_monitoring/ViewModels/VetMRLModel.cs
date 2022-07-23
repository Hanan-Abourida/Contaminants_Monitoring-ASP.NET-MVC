using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Contaminants_Monitoring.ViewModels
{
    public class VetMRLModel
    {
        public int pkVeterinaryResidueId { get; set; }
        public string VetResidueName { get; set; }
        public string AntibioticsClass { get; set; }

        public double mrl { get; set; }
        public string mrl_uom { get; set; }
        public string commodityName { get; set; }

        public int? pkVetMRLId { get; set; }
    }
}