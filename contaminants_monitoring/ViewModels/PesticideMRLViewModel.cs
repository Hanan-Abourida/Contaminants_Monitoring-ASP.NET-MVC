using Contaminants_Monitoring.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Contaminants_Monitoring.ViewModels
{
    public class PesticideMRLViewModel
    {
        public int pkResidueId { get; set; }
        public string residueName { get; set; }
        public string code { get; set; }
        public string adi { get; set; }
        public string arfd { get; set; }

        public bool? banned { get; set; }
        public string reference { get; set; }
        public double? mrl { get; set; }
        public string commodityName { get; set; }
        public string commodityCode { get; set; }

        public string mrl_uom { get; set; }
        public string adi_uom { get; set; }
        public string arfd_uom { get; set; }

        // For Create
        public List<PesticideResidue> pesticidesList { get; set; }
        public int selectedResID { get; set; }
        public List<UOM> units { get; set; }
        public int? selectedUOM { get; set; }
        public List<Commodity> commoditiesList { get; set; }
        public int selectedCommodityID { get; set; }

        //For Edit
        public int pkPRMRLId { get; set; }
    }
}