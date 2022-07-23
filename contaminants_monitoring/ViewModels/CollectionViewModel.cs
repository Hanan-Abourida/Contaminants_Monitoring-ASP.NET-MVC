using Contaminants_Monitoring.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Contaminants_Monitoring.ViewModels
{
    public class CollectionViewModel
    {
        public LabSample labSample { get; set; }
        public string collectorId { get; set; }

        public List<SamplingPlan> samplingPlans { get; set; }
        public int? selectedSamplingPlanId { get; set; }

        public List<CommodityState> commodityStates { get; set; }
        public int? selectedCommodityStateId { get; set; }

        public List<SamplingReason> samplingReasons { get; set; }
        public int? selectedSamplingReasonId { get; set; }

        public List<SamplingType> samplingTypes { get; set; }
        public int? selectedSamplingTypeId { get; set; }

        public List<string> premiseTypes { get; set; }
        public string selectedPremiseType { get; set; }

        public List<SourceType> sourceTypes { get; set; }
        public int? selectedSourceType { get; set; }

        public List<PackagingType> packagingTypes { get; set; }
        public int? selectedPackagingTypeId { get; set; }

        public List<SampleOrigin> sampleOrigins { get; set; }
        public int? selectedSampleOriginId { get; set; }

        public List<Country> originCountries { get; set; }
        public int? selectedOriginCountryId { get; set; }
    }
}