using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Contaminants_Monitoring.Models
{
    public class LabSampleExcelMapping
    {
        public string SampleNb { get; set; }
        public string LabSampleNb { get; set; }
        public string OfficerName { get; set; }
        public string OfficerPhoneNb { get; set; }
        public string GovernorateCode { get; set; }
        public string Caza { get; set; }
        public string SamplingPlanNb { get; set; }
        public string SampleCode { get; set; }
        public string CommNb { get; set; }
        public string CommName { get; set; }
        public string CommDescription { get; set; }
        public string CommClass { get; set; }
        public string CommGroupCode { get; set; }
        public string CommTypeCode { get; set; }
        public string SamplingReason { get; set; }
        public string SamplingType { get; set; }
        public string PremiseName { get; set; }
        public string PremiseType { get; set; }
        public string PremiseAddress { get; set; }
        public string ContactName { get; set; }
        public string ContactPhoneNumber { get; set; }
        public string SamplingDate { get; set; }
        public string SamplingTime { get; set; }
        public string BrandName { get; set; }
        public string Manufacturer { get; set; }
        public string Distributor { get; set; }
        public string Importer { get; set; }
        public string Origin { get; set; }
        public string CountryOfOrigin { get; set; }
        public string PackagingType { get; set; }
        public string PackQuantitySize { get; set; }
        public string BatchLotNumber { get; set; }
        public string ShelfLife { get; set; }
        public string StorageConditions { get; set; }
        public string LabCode { get; set; }
        public string SampleQuantity { get; set; }
        public string ReceivingDate { get; set; }
        public string ReceivingTime { get; set; }
        public string AnalysisDate { get; set; }
        public string Chemical { get; set; }
        public string PestCode { get; set; }
        public string PestName { get; set; }
        public string TestClass { get; set; }

        public string AnalysisPortion { get; set; }
        public string AnalysisType { get; set; }
        public string ConFinal { get; set; }
        public string ConUnit { get; set; }
        public string LOD { get; set; }
        public string LOQ { get; set; }
        public string Uncertainty { get; set; }
        public string UncertaintyPercent { get; set; }
        public string ExtractionMethod { get; set; }
        public string DeterminativeMethod { get; set; }
        public string ViolationType { get; set; }

    }
}