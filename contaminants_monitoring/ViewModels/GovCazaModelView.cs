using Contaminants_Monitoring.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Contaminants_Monitoring.ViewModels
{
    public class GovCazaModelView
    {
        public List<Governorate> governorates { get; set; }
        public List<Caza> cazas { get; set; }
    }
}