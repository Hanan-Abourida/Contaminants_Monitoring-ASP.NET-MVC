using Contaminants_Monitoring.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Contaminants_Monitoring.ViewModels
{
    public class DesignViewModel
    {
        //public Commodity commodity { get; set; }
        [Required(ErrorMessage ="The Commodity field is required.")]
        public int selectedCommodityId { get; set; }
        //public ContaminantType ContaminantType { get; set; }
        [Required(ErrorMessage = "The Contaminant Type field is required.")]
        public int selectedContaminantTypeId { get; set; }
        [Required(ErrorMessage = "The Samples Size field is required.")]
        public int SamplesSize { get; set; }
        //public Governorate governorate { get; set; }
        [Required(ErrorMessage = "The Governorate field is required.")]
        public int selectedGovernorateId { get; set; }
        //public Caza caza { get; set; }
        [Required(ErrorMessage = "The Caza field is required.")]
        public int selectedCazaId { get; set; }
        public string Notes { get; set; }
        //public List<LabSample> generatedLabSamples { get; set; }

        public List<SamplingReason> samplingReasons { get; set; }
        public int? selectedSamplingReasonId { get; set; }

        public List<SamplingPlan> samplingPlans { get; set; }
        public int? selectedSamplingPlanId { get; set; }

        public List<GeneratedSamplesViewModel> generatedSamplesViews { get; set; }

        public int fkDesignerId { get; set; }
        public int fkCollectorId { get; set; }
        public DateTime generationDate { get; set; }

        //preload values
        public List<Commodity> commodities { get; set; }
        public List<ContaminantType> contaminantTypes { get; set; }
        public List<Governorate> governorates { get; set; }
        public List<Caza> cazas { get; set; }
    }

    public class GeneratedSamplesViewModel{

        public Commodity commodity { get; set; }
        public ContaminantType ContaminantType { get; set; }
        public Governorate governorate { get; set; }
        public Caza caza { get; set; }
        public string Notes { get; set; }
        public string Designer { get; set; }
        public string Collector { get; set; }
        public DateTime generationDate { get; set; }

        public Laboratory laboratory { get; set; }
        public string SampleIdentityCode { get; set; }
        public int fkLabSampleId { get; set; }
    }
}