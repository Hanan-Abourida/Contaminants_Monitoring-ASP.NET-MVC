using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Contaminants_Monitoring.Models
{
    public class MetaData
    {
        public class LabSampleMetaData
        {
            [Display(Name = "Sample Number")]
            public string SampleNb { get; set; }

            [Display(Name = "Lab Sample Number")]
            public string LabSampleNb { get; set; }

            [Display(Name = "Officer Name")]
            public string OfficerName { get; set; }
            [Display(Name = "Officer Phone Number")]
            public string OfficerPhoneNb { get; set; }
            [Display(Name = "Governorate")]
            public Nullable<int> fkGovernorateId { get; set; }
            [Display(Name = "Caza")]
            public Nullable<int> fkCazaId { get; set; }
            [Display(Name = "Sampling Plan")]
            public string SamplingPlan { get; set; }

            [Display(Name = "Sample Code")]
            public string SampleCode { get; set; }

            [Display(Name = "Commodity State")]
            public Nullable<int> fkCommodityStateId { get; set; }

            [Display(Name = "Sampling Reason")]
            public Nullable<int> fkSamplingReasonId { get; set; }

            [Display(Name = "Sampling Type")]
            public Nullable<int> fkSamplingTypeId { get; set; }

            [Display(Name = "Premise Name")]
            public string PremiseName { get; set; }

            [Display(Name = "Point of Collection")]
            public string PremiseType { get; set; }

            [Display(Name = "Premise Address")]
            public string PremiseAddress { get; set; }

            [Display(Name = "Contact Name")]
            public string ContactName { get; set; }

            [Display(Name = "Contact Phone Number")]
            public string ContactPhoneNumber { get; set; }

            [Display(Name = "Sampling Date")]
            [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
            public Nullable<System.DateTime> SamplingDate { get; set; }

            [Display(Name = "Sampling Time")]
            //[DataType(DataType.Time)]
            //[DisplayFormat(DataFormatString = "{0:t}", ApplyFormatInEditMode = false)]
            public string SamplingTime { get; set; }

            [Display(Name = "Brand Name")]
            public string BrandName { get; set; }

            [Display(Name = "Manufacturer")]
            public string Manufacturer { get; set; }

            [Display(Name = "Distributer")]
            public string Distributor { get; set; }

            [Display(Name = "Importer")]
            public string Importer { get; set; }

            [Display(Name = "Sample Origin")]
            public Nullable<int> fkSampleOriginId { get; set; }

            [Display(Name = "Origin  Country")]
            public Nullable<int> fkOriginCountryId { get; set; }

            [Display(Name = "Packaging Type")]
            public Nullable<int> fkPackagingTypeId { get; set; }

            [Display(Name = "Package Quantity Size")]
            public string PackQuantitySize { get; set; }

            [Display(Name = "Batch Lot Number")]
            public string BatchLotNumber { get; set; }

            [Display(Name = "Shelf Life")]
            public string ShelfLife { get; set; }

            [Display(Name = "Storage Conditions")]
            public string StorageConditions { get; set; }

            [Display(Name = "Laboratory")]
            public Nullable<int> fkLaboratoryId { get; set; }

            [Display(Name = "Sample Quantity")]
            public string SampleQuantity { get; set; }

            [Display(Name = "Receiving Date")]
            [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
            public Nullable<System.DateTime> ReceivingDate { get; set; }

            [Display(Name = "Receiving Time")]
            public string ReceivingTime { get; set; }

            [Display(Name = "Analysis Date")]
            [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
            public Nullable<System.DateTime> AnalysisDate { get; set; }
            [Display(Name = "Dispatch Date")]
            [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
            public Nullable<System.DateTime> DispatchDate { get; set; }
            [Display(Name = "Close Case Date")]
            [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
            public Nullable<System.DateTime> CloseCaseDate { get; set; }
            [Display(Name = "Collection Due Date")]
            [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
            public Nullable<System.DateTime> Collection_Due_Date { get; set; }
            [Display(Name = "Collection Start Date")]
            [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
            public Nullable<System.DateTime> Collection_Start_Date { get; set; }

            [Display(Name = "Chemical")]
            public Nullable<int> fkContaminantTypeId { get; set; }

            [Display(Name = "Pesticide Residue")]
            public Nullable<int> fkPesticideResidueId { get; set; }

            [Display(Name = "Mycotoxin")]
            public Nullable<int> fkMycotoxinId { get; set; }

            [Display(Name = "Analysis Portion")]
            public string AnalysisPortion { get; set; }

            [Display(Name = "Analysis Type")]
            public string AnalysisType { get; set; }

            [Display(Name = "Final Concentration")]
            public string ConFinal { get; set; }

            [Display(Name = "Unit")]
            public Nullable<int> fkConUOM { get; set; }

            [Display(Name = "LOD")]
            public Nullable<double> LOD { get; set; }

            [Display(Name = "LOQ")]
            public Nullable<double> LOQ { get; set; }

            [Display(Name = "Uncertainty")]
            public Nullable<double> Uncertainty { get; set; }

            [Display(Name = "UncertaintyPercent")]
            public Nullable<double> UncertaintyPercent { get; set; }

            [Display(Name = "Extraction Method")]
            public string ExtractionMethod { get; set; }

            [Display(Name = "Determinative Method")]
            public string DeterminativeMethod { get; set; }

            [Display(Name = "ViolationType")]
            public string ViolationType { get; set; }

            [Display(Name = "Commodity")]
            public Nullable<int> fkCommodityId { get; set; }
        }
    }
}