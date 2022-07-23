//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Contaminants_Monitoring.Models
{
    using System;
    
    public partial class GetGeneratedSamples_Result
    {
        public Nullable<int> pkLabSampleId { get; set; }
        public string SampleNb { get; set; }
        public string LabSampleNb { get; set; }
        public string OfficerName { get; set; }
        public string OfficerPhoneNb { get; set; }
        public Nullable<int> fkGovernorateId { get; set; }
        public Nullable<int> fkCazaId { get; set; }
        public Nullable<int> SamplingPlan { get; set; }
        public string SampleCode { get; set; }
        public Nullable<int> fkCommodityStateId { get; set; }
        public Nullable<int> fkSamplingReasonId { get; set; }
        public Nullable<int> fkSamplingTypeId { get; set; }
        public string PremiseName { get; set; }
        public string PremiseType { get; set; }
        public string PremiseAddress { get; set; }
        public string ContactName { get; set; }
        public string ContactPhoneNumber { get; set; }
        public Nullable<System.DateTime> SamplingDate { get; set; }
        public string SamplingTime { get; set; }
        public string BrandName { get; set; }
        public string Manufacturer { get; set; }
        public string Distributor { get; set; }
        public string Importer { get; set; }
        public Nullable<int> fkSampleOriginId { get; set; }
        public Nullable<int> fkOriginCountryId { get; set; }
        public Nullable<int> fkPackagingTypeId { get; set; }
        public string PackQuantitySize { get; set; }
        public string BatchLotNumber { get; set; }
        public string ShelfLife { get; set; }
        public string StorageConditions { get; set; }
        public Nullable<int> fkLaboratoryId { get; set; }
        public string SampleQuantity { get; set; }
        public Nullable<System.DateTime> ReceivingDate { get; set; }
        public string ReceivingTime { get; set; }
        public Nullable<System.DateTime> AnalysisDate { get; set; }
        public Nullable<int> fkContaminantTypeId { get; set; }
        public Nullable<int> fkPesticideResidueId { get; set; }
        public Nullable<int> fkMycotoxinId { get; set; }
        public string AnalysisPortion { get; set; }
        public string AnalysisType { get; set; }
        public string ConFinal { get; set; }
        public Nullable<int> fkConUOM { get; set; }
        public Nullable<double> LOD { get; set; }
        public Nullable<double> LOQ { get; set; }
        public Nullable<double> Uncertainty { get; set; }
        public Nullable<double> UncertaintyPercent { get; set; }
        public string ExtractionMethod { get; set; }
        public string DeterminativeMethod { get; set; }
        public string ComplianceResult { get; set; }
        public Nullable<int> fkCommodityId { get; set; }
        public Nullable<System.DateTime> DispatchDate { get; set; }
        public Nullable<System.DateTime> CloseCaseDate { get; set; }
        public string Notes_Designer { get; set; }
        public string Notes_Collector { get; set; }
        public string Lab_Rejection_Comment { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> Collection_Due_Date { get; set; }
        public Nullable<System.DateTime> Collection_Start_Date { get; set; }
        public Nullable<int> fkSourceTypeId { get; set; }
        public string SourceName { get; set; }
        public string SourceAddress { get; set; }
        public string SourceContactName { get; set; }
        public string SourceContactNumber { get; set; }
        public string CollectedQuantity { get; set; }
        public string ViolationType { get; set; }
        public Nullable<int> pkCollectionDesignId { get; set; }
        public Nullable<int> fkLabSampleId { get; set; }
        public string fkDesignerId { get; set; }
        public Nullable<System.DateTime> GeneratedDate { get; set; }
        public string fkCollectorId { get; set; }
        public Nullable<int> pkLaboratoryId { get; set; }
        public string LaboratoryName { get; set; }
        public string LaboratoryCode { get; set; }
        public string LabPhoneNumber { get; set; }
        public string LabEmail { get; set; }
        public string LabAddress { get; set; }
    }
}
