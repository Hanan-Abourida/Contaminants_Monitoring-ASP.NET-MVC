using Contaminants_Monitoring.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Contaminants_Monitoring.ViewModels
{
    public class MRLViewModel
    {
        public PestResMRL pestResMRL{get; set;}
        public PesticideResidue pesticideResidue { get; set; }
        public string uom_MRL { get; set; }
        public string uom_ADI { get; set; }
        public string uom_ARFD { get; set; }

        public int pkId { get; set; }

    }
}