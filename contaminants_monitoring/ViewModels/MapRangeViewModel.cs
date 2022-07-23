using Contaminants_Monitoring.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Contaminants_Monitoring.ViewModels
{
    public class MapRangeViewModel
    {
        public string selectedFromNb { get; set; }
        public string selectedToNb { get; set; }
        public List<string> listFromNb { get; set; }
        public List<string> listToNb { get; set; }

        [Required(ErrorMessage = "The Governorate field is required.")]
        public int selectedGovernorateId { get; set; }
        //public Caza caza { get; set; }
        [Required(ErrorMessage = "The Caza field is required.")]
        public int selectedCazaId { get; set; }
        public List<Governorate> governorates { get; set; }
        public List<Caza> cazas { get; set; }

        public List<SamplingPlan> samplingPlans { get; set; }
        [Required(ErrorMessage = "The Sampling plan field must be selected.")]
        public int? selectedSamplingPlanId { get; set; }


        public List<Laboratory> Laboratories { get; set; }
        public List<ApplicationUser> collectors { get; set; }

       // public ApplicationUser selectedApplicationUser { get; set; }
        public int selectedLabId { get; set; }
        public string selectedCollectorId { get; set; }

        public Nullable<System.DateTime> DueDate { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
    }
}