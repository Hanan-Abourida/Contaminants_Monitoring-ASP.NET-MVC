using Contaminants_Monitoring.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using static Contaminants_Monitoring.ViewModels.CommodityHistoryVM;

namespace Contaminants_Monitoring.Models
{
    public class DataAccess
    {
        private static FoodSafetyDBEntities db = new FoodSafetyDBEntities();

        public static List<Governorate> GetAllGovernorates()
        {
            return db.Governorates.ToList();
        }

        public static List<Caza> GetAllCazas()
        {
            return db.Cazas.ToList();
        }

        public static Commodity getCommodityFromId(int? commodityId)
        {
            if(commodityId != null)
            {
                return db.Commodities.Where(c => c.pkCommodityId == commodityId).First();
            }
            return null;
        }

        public static int GetGovernorateIdFromText(string governorateText)
        {
            int pkGovernorateId = 0;

            if (!string.IsNullOrEmpty(governorateText))
            {
                pkGovernorateId = db.Governorates.Where(x => x.GovernorateCode == governorateText
                                                             || x.GovernorateName == governorateText)
                                                             .ToList().FirstOrDefault().pkGovernorateId;
            }
            return pkGovernorateId;
        }
        public static Governorate GetGovernorateFromId(int pkGovernorateId)
        {
            Governorate governorate = new Governorate();
            governorate = db.Governorates.Where(x => x.pkGovernorateId == pkGovernorateId).ToList().FirstOrDefault();
            return governorate;
        }
        public static Caza GetCazaFromId(int pkCazaId)
        {
            return db.Cazas.Where(x => x.pkCazaId == pkCazaId).ToList().FirstOrDefault();
        }
        public static ContaminantType GetContaminantTypeFromId(int pkContaminantTypeId)
        {
            return db.ContaminantTypes.Where(x => x.pkContaminantTypeId == pkContaminantTypeId).ToList().FirstOrDefault();
        }

        public static int GetCazaIdFromText(string cazaText)
        {
            int pkCazaId = 0;

            if (!string.IsNullOrEmpty(cazaText))
            {
                pkCazaId = db.Cazas.Where(x => x.CazaCode == cazaText
                                                             || x.CazaName == cazaText)
                                                             .ToList().FirstOrDefault().pkCazaId;
            }
            return pkCazaId;
        }

        public static int GetUOMIdFromText(string UOMText)
        {
            int pkUOMId = 0;

            if (!string.IsNullOrEmpty(UOMText))
            {
                pkUOMId = db.UOMs.Where(x => x.unit == UOMText)
                                                             .ToList().FirstOrDefault().pkUOMId;
            }
            return pkUOMId;
        }

        public static string GetUOMTextFromId(int UOMId)
        {
            string uom = "";

            if (UOMId > 0)
            {
                uom = db.UOMs.Where(x => x.pkUOMId == UOMId)
                                                             .ToList().FirstOrDefault().unit;
            }
            return uom;
        }

        public static int GetSamplingReasonIdFromText(string SamplingResaonText)
        {
            int pkSamplingReasonId = 0;

            if (!string.IsNullOrEmpty(SamplingResaonText))
            {
                pkSamplingReasonId = db.SamplingReasons.Where(x => x.SamplingReasonCode == SamplingResaonText || x.SamplingReasonText == SamplingResaonText)
                                                             .ToList().FirstOrDefault().pkSamplingReasonId;
            }
            return pkSamplingReasonId;
        }

        public static int GetSamplingTypeIdFromText(string SamplingTypeText)
        {
            int pkSamplingTypeId = 0;

            if (!string.IsNullOrEmpty(SamplingTypeText))
            {
                pkSamplingTypeId = db.SamplingTypes.Where(x => x.SamplingTypeCode == SamplingTypeText || x.SamplingTypeText == SamplingTypeText)
                                                             .ToList().FirstOrDefault().pkSamplingTypeId;
            }
            return pkSamplingTypeId;
        }

        public static int GetCountryIdFromText(string countryText)
        {
            int pkCountryId = 0;

            if (!string.IsNullOrEmpty(countryText))
            {
                pkCountryId = db.Countries.Where(x => x.CountryName == countryText)
                                                             .ToList().FirstOrDefault().pkCountryId;
            }
            return pkCountryId;
        }

        public static int GetPackagingTypeIdFromText(string packagingTypeText)
        {
            int pkPackagingTypeId = 0;

            if (!string.IsNullOrEmpty(packagingTypeText))
            {
                pkPackagingTypeId = db.PackagingTypes.Where(x => x.PackagingTypeText == packagingTypeText
                || x.PackagingTypeCode == packagingTypeText).ToList().FirstOrDefault().pkPackagingTypeId;
            }
            return pkPackagingTypeId;
        }

        public static int GetLabIdFromText(string LabText)
        {
            int pkLabId = 0;

            if (!string.IsNullOrEmpty(LabText))
            {
                pkLabId = db.Laboratories.Where(x => x.LaboratoryCode == LabText || x.LaboratoryName == LabText)
                                                             .ToList().FirstOrDefault().pkLaboratoryId;
            }
            return pkLabId;
        }

        public static int GetOriginIdFromText(string OriginText)
        {
            int pkOriginId = 0;

            if (!string.IsNullOrEmpty(OriginText))
            {
                pkOriginId = db.SampleOrigins.Where(x => x.OriginCode == OriginText || x.OriginText == OriginText)
                                                             .ToList().FirstOrDefault().pkSampleOriginId;
            }
            return pkOriginId;
        }

        public static List<Commodity> getAllCommodities()
        {
            return db.Commodities.ToList();
        }

        public static List<ContaminantType> GetAllContaminantsTypes()
        {
            return db.ContaminantTypes.ToList();
        }

        public static int GetCommodityStateId(int commodityId, string commDescription)
        {
            if (commodityId == 6)//If Pistachio, its state is always ready to eat
                return 13;
            int commodityStateId = 0;
            if (!String.IsNullOrEmpty(commDescription))
            {
                //first check if exist in list of states related to specific commodity
                if (checkDescriptionExists(commodityId,commDescription))
                    commodityStateId = db.CommodityStates.Where(x => x.fkCommodityId == commodityId && x.CommodityDescription.Contains(commDescription))
                        .ToList().FirstOrDefault().pkCommodityStateId;
                else commodityStateId = addNewDescriptionToCommodity(commodityId,commDescription);
            }
            return commodityStateId;
        }

        private static bool checkDescriptionExists(int commodityId, string commDescription)
        {
            List<CommodityState> allCommStates = db.CommodityStates.Where(x => x.fkCommodityId == commodityId).ToList();
            foreach (CommodityState desc in allCommStates)
            {
                if (desc.CommodityDescription.Contains(commDescription))
                    return true;
            }
            return false;
        }
        private static int addNewDescriptionToCommodity(int commodityId, string newDescription)
        {
            CommodityState newCommodityState = new CommodityState();
            newCommodityState.fkCommodityId = commodityId;
            newCommodityState.CommodityDescription = newDescription;
            db.CommodityStates.Add(newCommodityState);

            db.SaveChanges();
            return db.CommodityStates.Where(x => x.fkCommodityId == commodityId && x.CommodityDescription.Contains(newDescription))
                        .ToList().FirstOrDefault().pkCommodityStateId;
        }

        public static int GetContaminantTypeIdFromText(string contaminantTypeText) 
        {
            int pkContaminantTypeId = 0;
            if (string.IsNullOrEmpty(contaminantTypeText) || string.IsNullOrEmpty(contaminantTypeText))
            {
                //add warning message that contaminant type column is empty
                return 0;
            }
            pkContaminantTypeId = db.ContaminantTypes.Where(x => x.ContaminantCode == contaminantTypeText)
                        .ToList().FirstOrDefault().pkContaminantTypeId;
            return pkContaminantTypeId;
        }

        public static int GetMycotoxinIdFromText(string mycotoxinName)
        {
            int myCotoxinId = 0;
            if (!string.IsNullOrEmpty(mycotoxinName.Trim()))
            {
                myCotoxinId = db.Mycotoxins.Where(x => x.MycotoxinName == mycotoxinName)
                    .ToList().FirstOrDefault().pkMycotoxinId;
            }
            return myCotoxinId;
        }

        public static int GetPesticideResidueIdFromText(string pesticideResidueName)
        {
            int pesticideResidueId = 0;
            if (!string.IsNullOrEmpty(pesticideResidueName))
            {
                List<PesticideResidue> result = db.PesticideResidues.Where(x => x.PestResName == pesticideResidueName)
                    .ToList();
                if (result.Count != 0)
                    pesticideResidueId = result.FirstOrDefault().pkPesticideResidueId;
                else return -1;
            }
            return pesticideResidueId;

        }
        public static PesticideResidue GetPesticideResidueFromId(int pesticideResidueId)
        {
           PesticideResidue result = db.PesticideResidues.Where(x => x.pkPesticideResidueId == pesticideResidueId)
                    .ToList().First();
            return result;

        }
        public static Mycotoxin GetMycotoxinFromId(int mycotoxinId)
        {
            Mycotoxin result = db.Mycotoxins.Where(x => x.pkMycotoxinId == mycotoxinId)
                     .ToList().First();
            return result;

        }

        public static string SetResidueViolationType(double conFinal, int commodityId, int? pestResId)
        {
            string result = "";
            double targetMRL = db.PestResMRLs.Where(x=>x.fkPesticideResidueId == pestResId && x.fkCommodityId == commodityId)
                .ToList().FirstOrDefault().MRL;
            if (conFinal < targetMRL)
            {
                result = "C";
            }
            if (conFinal >= targetMRL)
            {
                result = "V";
            }
            return result;
        }

        public static bool ResidueIsBanned(int PesticideResidueId)
        {
            var result = db.PesticideResidues.Where(x => x.pkPesticideResidueId == PesticideResidueId && x.Banned != null && x.Banned == true).ToList();
            if ( result != null && result.Count > 0)
            {
                return true;
            }
            else return false;
        }

        public static string SetMycotoxinViolationType(double conFinal, int commodityId, int? myCotoxinId)
        {
            string result = "";
            double targetML = db.MycotoxinMLs.Where(x => x.fkMycotoxinId == myCotoxinId && x.fkCommodityId == commodityId)
               .ToList().FirstOrDefault().ML;
            if (conFinal < targetML)
            {
                result = "C";
            }
            if (conFinal >= targetML)
            {
                result = "V";
            }
            return result;
        }

        public static bool AddNewLabSample(List<LabSample> labSamples)
        {
            db.LabSamples.AddRange(labSamples);
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {

                return false;
            }
            return true;
        }
        public static bool AddNewSingleLabSample(LabSample labSample)
        {
            db.LabSamples.Add(labSample);
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {

                return false;
            }
            return true;
        }

        public static bool UpdateLabTestInfo(LabSample labSample)
        {
            db.Entry(labSample).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {

                return false;
            }
            return true;
        }

        //For search page
        public static List<LabSample> getAllLabSamples()
        {
            List<LabSample> labSamples = new List<LabSample>();
            labSamples = db.LabSamples.ToList();
            return labSamples;
        }

        public static List<PesticideResidue> getAllPesticideResidues()
        {
            List<PesticideResidue> pesticideResidues = new List<PesticideResidue>();
            pesticideResidues = db.PesticideResidues.ToList();
            return pesticideResidues;
        }

        public static List<Mycotoxin> getAllMycotoxins()
        {
            List<Mycotoxin> mycotoxins = new List<Mycotoxin>();
            mycotoxins = db.Mycotoxins.ToList();
            return mycotoxins;
        }

        public static List<LabSample> FilterSamplesByCommodities(List<int?> commoditiesIds)
        {
            List<LabSample> result = new List<LabSample>();
            foreach(int id in commoditiesIds)
            {
                result.AddRange(db.LabSamples.Where(s => s.fkCommodityId == id).ToList());
            }
            return result;
        }

        public static List<LabSample> filterSamplesByPesticides(List<int?> PRIds, List<LabSample> labSamples)
        {
            List<LabSample> result = new List<LabSample>();
            foreach (int i in PRIds)
            {
                result.AddRange(labSamples.Where(s => s.fkPesticideResidueId == i));
            }
            return result;
        }

        public static List<LabSample> filterSamplesByMycotoxin(List<int?> myCotoxins, List<LabSample> labSamples)
        {
            List<LabSample> result = new List<LabSample>();
            foreach (int i in myCotoxins)
            {
                result.AddRange(labSamples.Where(s => s.fkMycotoxinId == i));
            }
            return result;
        }

        public static List<LabSample> filterSamplesByYears(List<int?> years, List<LabSample> labSamples)
        {
            List<LabSample> result = new List<LabSample>();
            foreach (int i in years)
            {
                result.AddRange(labSamples.Where(s=>s.ReceivingDate != null && s.ReceivingDate.Value.Year == i));
            }
            return result;
        }

        public static List<LabSample> RemoveUndetectedResult(List<LabSample> filteredResult)
        {
            List<LabSample> result = new List<LabSample>();
            foreach(LabSample sample in filteredResult)
            {
                if (sample.ConFinal != "NQ" && sample.ConFinal != "ND" && sample.ConFinal != "0")
                    result.Add(sample);
            }
            return result;
        }

        public static List<LabSample> GetViolativeOnlyFromList(List<LabSample> filteredResult)
        {
            List<LabSample> result = new List<LabSample>();
            foreach (LabSample sample in filteredResult)
            {
                if (sample.ComplianceResult == "V")
                    result.Add(sample);
            }
            return result;
        }

        public static List<CommodityHistoryItem> GetCommodityHistory()
        {
            var distinctYears = getDistinctYearsOfAnalysis();
            var commodities = db.Commodities.ToList();
            List<CommodityHistoryItem> result = new List<CommodityHistoryItem>();
            foreach (Commodity commodity in commodities)
            {
                foreach (int year in distinctYears)
                {
                    CommodityHistoryItem item = new CommodityHistoryItem();
                    item.commodityName = commodity.CommodityCode + " - " + commodity.CommodityName;
                    item.year = year;
                    item.nbOfSample = db.LabSamples.Where(s => s.ReceivingDate.HasValue && s.fkCommodityId == commodity.pkCommodityId && s.ReceivingDate.Value.Year == year)
                        .ToList().Count;
                    result.Add(item);
                }
            }
            return result;
        }

        public static List<ContaminantHistoryItem> GetContaminantHistory()
        {
            var distinctYears = getDistinctYearsOfAnalysis();
            var contaminants = db.ContaminantTypes.ToList();
            List<ContaminantHistoryItem> result = new List<ContaminantHistoryItem>();
            foreach (ContaminantType contaminant in contaminants)
            {
                foreach (int year in distinctYears)
                {
                    ContaminantHistoryItem item = new ContaminantHistoryItem();
                    item.contaminant = contaminant.ContaminantName;
                    item.year = year;
                    int count = 0;
                    count  = (from a in db.LabSamples
                                 where a.ReceivingDate.HasValue && a.ReceivingDate.Value.Year == year 
                                 && a.fkContaminantTypeId == contaminant.pkContaminantTypeId
                                 select a.fkCommodityId).Distinct().ToList().Count;
                    item.nbOfProducts = count;
                    result.Add(item);
                }
            }
            return result;
        }

        private static List<int> getDistinctYearsOfAnalysis()
        {
            List<int> years = new List<int>();
            var distinctDates = (from a in db.LabSamples
                                 select a.ReceivingDate).Distinct().ToList();
            var distinctYears = (from d in distinctDates where d.HasValue select Convert.ToDateTime(d.Value).Year).Distinct();
            years = distinctYears.ToList();
            return years;
        }

        public static List<Governorate> GetDistinctGovernoratesInSamples(List<LabSample> labSamples)
        {
            var distinctgovernorates = (from a in labSamples
                                 select a.fkGovernorateId).Distinct().ToList();
            List<Governorate> result = new List<Governorate>();
            foreach(var gov in distinctgovernorates)
            {
                result.Add(db.Governorates.Where(g => g.pkGovernorateId == gov).First());
            }
            return result;
        }

        public static List<Commodity> GetDistinctCommoditiesInSamples(List<LabSample> labSamples)
        {
            var distinctCommodities = (from a in labSamples
                                        select a.fkCommodityId).Distinct().ToList();
            List<Commodity> result = new List<Commodity>();
            foreach (var comm in distinctCommodities)
            {
                result.Add(db.Commodities.Where(c => c.pkCommodityId == comm).First());
            }
            return result;
        }

        public static List<string> GetDistinctPremiseTypesForSamples(List<LabSample> labSamples)
        {
            List<string> distinctPremiseTypes = new List<string>();
            //if (labSamples != null && labSamples.Count > 0)
            //{
            //    var premiseTypes = (from a in labSamples
            //                        where a.PremiseType != null
            //                        select a.PremiseType.Trim()).Distinct().ToList();
            //    foreach (var pre in premiseTypes)
            //    {
            //        if (pre != null)
            //            distinctPremiseTypes.Add(pre);
            //    }
            //}
            distinctPremiseTypes.Add("Warehouse");
            distinctPremiseTypes.Add("Street Vendor");
            distinctPremiseTypes.Add("Farm");
            distinctPremiseTypes.Add("Supermarket");
            return distinctPremiseTypes;
        }

        public static List<SampleOrigin> GetDistinctOriginsForSamples(List<LabSample> labSamples)
        {
            List<SampleOrigin> distinctOrigins = new List<SampleOrigin>();
            if (labSamples != null && labSamples.Count > 0)
            {
                var origins = (from a in labSamples
                                    select a.fkSampleOriginId).Distinct().ToList();
                foreach (var origin in origins)
                {
                    if (origin != null)
                    {
                        SampleOrigin currentOrigin = db.SampleOrigins.Where(o => o.pkSampleOriginId == origin).FirstOrDefault();
                        distinctOrigins.Add(currentOrigin);
                    }
                        
                }
            }
            return distinctOrigins;
        }

        //public static List<MRLViewModel> GetAllMRLs()
        //{
        //    List<PestResMRL> allMRLs = db.PestResMRLs.ToList();
        //    List<MRLViewModel> allMRLs_VM = new List<MRLViewModel>();
        //    foreach(PestResMRL pestResMRL in allMRLs)
        //    {
        //        string current_uom = GetUOMTextFromId(pestResMRL.fkUOMId);
        //        MRLViewModel mRLViewModel = new MRLViewModel();
        //        mRLViewModel.uom = current_uom;
        //        mRLViewModel.pestResMRL = pestResMRL;
        //        allMRLs_VM.Add(mRLViewModel);
        //    }
        //    return allMRLs_VM;
        //}

        public static List<PesticideMRLViewModel> GetAllPesticidesAndMRLs()
        {
            List<PesticideMRLViewModel> allMRLs_VM = new List<PesticideMRLViewModel>();
            var result = (from m in db.PestResMRLs
                          join pr in db.PesticideResidues.DefaultIfEmpty() on m.fkPesticideResidueId equals pr.pkPesticideResidueId
                          select new
                          {
                              pkPRMRLId = m.pkPestResMRLId,
                              pkResidueId = pr.pkPesticideResidueId,
                              residueName = pr.PestResName,
                              code = pr.PestResCode,
                              adi = pr.ADI,
                              arfd = pr.ARFD,
                              banned = pr.Banned,
                              reference = pr.Reference,
                              mrl = m.MRL,
                              commodityName = m.Commodity.CommodityName,
                              commodityCode = m.Commodity.CommodityCode,
                              mrl_uom = m.fkUOMId,
                              adi_uom = pr.fkADI_UnitId,
                              arfd_uom = pr.fkARFD_UnitId
                          }).ToList();
            
            foreach (var item in result)
            {
                PesticideMRLViewModel mRLViewModel = new PesticideMRLViewModel();
                if (item.mrl_uom != 0)
                    mRLViewModel.mrl_uom = GetUOMTextFromId(item.mrl_uom);
                if(item.adi_uom != null)
                    mRLViewModel.adi_uom = GetUOMTextFromId((int)item.adi_uom);
                if (item.arfd_uom != null)
                    mRLViewModel.arfd_uom = GetUOMTextFromId((int)item.arfd_uom);

                mRLViewModel.pkPRMRLId = item.pkPRMRLId;
                mRLViewModel.pkResidueId = item.pkResidueId;
                mRLViewModel.residueName = item.residueName;
                mRLViewModel.reference = item.reference;
                mRLViewModel.code = item.code;
                mRLViewModel.adi = item.adi;
                mRLViewModel.arfd = item.arfd;
                mRLViewModel.banned = item.banned;
                mRLViewModel.mrl = item.mrl;
                mRLViewModel.commodityName = item.commodityName;
                mRLViewModel.commodityCode = item.commodityCode;
                allMRLs_VM.Add(mRLViewModel);
            }
            
            return allMRLs_VM;
        }

        public static PesticideMRLViewModel GetSinglePesticideAndMRL(int? PRMrlId)
        {
            var result = (from m in db.PestResMRLs
                          join pr in db.PesticideResidues.DefaultIfEmpty() on m.fkPesticideResidueId equals pr.pkPesticideResidueId
                          where m.pkPestResMRLId == PRMrlId
                          select new
                          {
                              pkPRMRLId = m.pkPestResMRLId,
                              pkResidueId = pr.pkPesticideResidueId,
                              residueName = pr.PestResName,
                              code = pr.PestResCode,
                              adi = pr.ADI,
                              arfd = pr.ARFD,
                              banned = pr.Banned,
                              reference = pr.Reference,
                              mrl = m.MRL,
                              commodityName = m.Commodity.CommodityName,
                              commodityCode = m.Commodity.CommodityCode,
                              mrl_uom = m.fkUOMId,
                              adi_uom = pr.fkADI_UnitId,
                              arfd_uom = pr.fkARFD_UnitId
                          }).FirstOrDefault();

                PesticideMRLViewModel mRLViewModel = new PesticideMRLViewModel();
                if (result.mrl_uom != 0)
                    mRLViewModel.mrl_uom = GetUOMTextFromId(result.mrl_uom);
                if (result.adi_uom != null)
                    mRLViewModel.adi_uom = GetUOMTextFromId((int)result.adi_uom);
                if (result.arfd_uom != null)
                    mRLViewModel.arfd_uom = GetUOMTextFromId((int)result.arfd_uom);
                mRLViewModel.pkPRMRLId = result.pkPRMRLId;
                mRLViewModel.pkResidueId = result.pkResidueId;
                mRLViewModel.residueName = result.residueName;
                mRLViewModel.reference = result.reference;
                mRLViewModel.code = result.code;
                mRLViewModel.adi = result.adi;
                mRLViewModel.arfd = result.arfd;
                mRLViewModel.banned = result.banned;
                mRLViewModel.mrl = result.mrl;
                mRLViewModel.commodityName = result.commodityName;
                mRLViewModel.commodityCode = result.commodityCode;

            return mRLViewModel;
        }

        public static List<MLViewModel> GetAllMLs()
        {
            List<MycotoxinML> allMLs = db.MycotoxinMLs.ToList();
            List<MLViewModel> allMLs_VM = new List<MLViewModel>();
            foreach (MycotoxinML mycotoxinML in allMLs)
            {
                string current_uom = GetUOMTextFromId(mycotoxinML.fkUOMId);
                MLViewModel mLViewModel = new MLViewModel();
                mLViewModel.uom = current_uom;
                mLViewModel.mycotoxinML = mycotoxinML;
                allMLs_VM.Add(mLViewModel);
            }
            return allMLs_VM;
        }

        //Generate Lab Samples Data Access

        public static bool AddNewCollectionDesignList(List<CollectionDesign> collectionDesigns)
        {
            db.CollectionDesigns.AddRange(collectionDesigns);
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                
                return false;
            }
            return true;
        }

        public static int GetLabSampleIdFromIdentityCode(string identityCode)
        {
            if (!String.IsNullOrEmpty(identityCode))
            {
                int pkLabSampleId = (from a in db.LabSamples
                                     where a.SampleNb == identityCode
                                     select a.pkLabSampleId).ToList().FirstOrDefault();
                if (pkLabSampleId > 0) return pkLabSampleId;
                else return 0;

            }
            else return 0;
        }

        public static LabSample GetLabSampleFromId(int? id)
        {
            LabSample labSample = db.LabSamples.Find(id);
            return labSample;
        }

        public static LabSample GetFirstLabSampleFromSampleNb(string identityCode)
        {
            LabSample labSample = db.LabSamples.Where(l=>l.SampleNb == identityCode).First();
            return labSample;
        }

        public static int GetMaxCodeForSpecificGovCaza(string governorateCode, string cazaCode)
        {
            string prefix = governorateCode + cazaCode;
            int max = 0;
            List<string> codes = (from a in db.LabSamples
                        where a.SampleNb.StartsWith(prefix)
                        select a.SampleNb).ToList();
            if (codes.Count > 0)
            {
                List<int> codes_int = new List<int>();
                foreach (string code in codes)
                {
                    int code_int = int.Parse(code.Remove(0, prefix.Length));
                    codes_int.Add(code_int);
                }
                max = Max(codes_int);
            }
            
            return max;
        }

        private static int Max(IEnumerable<int> source)
        {
            if (source == null)
                throw new NullReferenceException("source is null");
            int num1 = 0;
            bool flag = false;
            foreach (int num2 in source)
            {
                if (flag)
                {
                    if (num2 > num1)
                        num1 = num2;
                }
                else
                {
                    num1 = num2;
                    flag = true;
                }
            }
            if (flag) { return num1; }
            throw new NullReferenceException("No Elements in source");
        }

        public static List<Laboratory> GetLaboratories()
        {
            return db.Laboratories.ToList();
        }

        public static Laboratory GetLaboratoryFromId(int? id)
        {
            if (id != null)
                return db.Laboratories.Where(l => l.pkLaboratoryId == id).FirstOrDefault();
            return null;
        }

        public static bool AddCollectorAndLabToSampleDesign(int labSampleId, ApplicationUser collector, int selectedLabId, DateTime? dueDate, DateTime? startDate)
        {
            CollectionDesign collectionDesign = db.CollectionDesigns.Where(c => c.fkLabSampleId == labSampleId).FirstOrDefault();
            collectionDesign.fkCollectorId = collector.Id;
            db.Entry(collectionDesign).State = EntityState.Modified;

            LabSample labSample = DataAccess.GetLabSampleFromId(labSampleId);
            labSample.fkLaboratoryId = selectedLabId;
            labSample.Collection_Due_Date = dueDate;
            labSample.Collection_Start_Date = startDate;
            db.Entry(labSample).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }catch(Exception e)
            {
                return false;
            }
            return true;
        }

        public static List<SampleStatusViewModel> GetAllLabSamplesWithDesign()
        {
            List<SampleStatusViewModel> result = new List<SampleStatusViewModel>();
            List<GetGeneratedSamples_Result> getGeneratedSamples_Results = db.GetGeneratedSamples().ToList();
           
            //var temp = (from s in db.LabSamples
            //            join d in db.CollectionDesigns on s.pkLabSampleId equals d.fkLabSampleId
            //           join l in db.Laboratories on s.fkLaboratoryId equals l.pkLaboratoryId
            //           select new
            //           {
            //               labSampleId = s.pkLabSampleId,
            //               identityCode = s.SampleNb,
            //               status = s.Status,
            //               commodity = s.Commodity.CommodityName,
            //               contaminantType = s.ContaminantType.ContaminantName,
            //               designerNotes = s.Notes_Designer,
            //               collector = d.fkCollectorId,
            //               //collector = UsersDataAccess.GetCollectorFromId(d.fkCollectorId).UserName,
            //               laboratoty = s.Laboratory.LaboratoryName,
            //               generatedDate = d.GeneratedDate,
            //               governorate = s.Governorate.GovernorateName,
            //               caza = s.Caza.CazaName,
            //               collectionNotes = s.Notes_Collector,
            //               dispatchDate = s.DispatchDate
            //           }).ToList();
            foreach(var item in getGeneratedSamples_Results)
            {
                SampleStatusViewModel newSampleStatusVM = new SampleStatusViewModel();
                newSampleStatusVM.labSampleId = (int)item.pkLabSampleId;
                newSampleStatusVM.identityCode = item.SampleNb;
                newSampleStatusVM.status = item.Status;
                Commodity commodity = getCommodityFromId(item.fkCommodityId);
                newSampleStatusVM.commodity = commodity.CommodityName + " - " + commodity.CommodityCode;
                newSampleStatusVM.contaminantType = GetContaminantTypeFromId((int)item.fkContaminantTypeId).ContaminantName;
                newSampleStatusVM.designerNotes = item.Notes_Designer;
                if(item.fkCollectorId != null)
                    newSampleStatusVM.collector = UsersDataAccess.GetCollectorFromId(item.fkCollectorId).UserName;
                newSampleStatusVM.laboratoty = item.LaboratoryName;
                newSampleStatusVM.generatedDate = item.GeneratedDate.HasValue ? item.GeneratedDate.Value.ToShortDateString() : "";
                newSampleStatusVM.governorate = db.Governorates.Find(item.fkGovernorateId).GovernorateName;
                newSampleStatusVM.caza = db.Cazas.Find(item.fkCazaId).CazaName;
                newSampleStatusVM.collectionNotes = item.Notes_Collector;
                newSampleStatusVM.dispatchDate = item.DispatchDate.HasValue ? item.DispatchDate.Value.ToShortDateString() : " ";
                result.Add(newSampleStatusVM);
            }
            return result;
        }

        public static bool DispatchLabSamples(List<LabSample> selectedSamples)
        {
            foreach(LabSample labSample in selectedSamples)
            {
                labSample.Status = "Dispatched";
                labSample.DispatchDate = DateTime.Now.Date;
                db.Entry(labSample).State = EntityState.Modified;
            }
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        //public static bool DeclareSampleCollection(int id, string notes)
        //{
        //    LabSample labSample = GetLabSampleFromId(id);
        //    labSample.SamplingDate = DateTime.Now.Date;
        //    labSample.Status = "Collected";
        //    db.Entry(labSample).State = EntityState.Modified;
        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (Exception e)
        //    {
        //        return false;
        //    }
        //    return true;
        //}

        public static bool saveCollectionData(int id, LabSample editedLabSample)
        {
            LabSample labSample = GetLabSampleFromId(id);

            labSample.SamplingPlan = editedLabSample.SamplingPlan;
            labSample.Notes_Collector = editedLabSample.Notes_Collector;
            labSample.fkCommodityStateId = editedLabSample.fkCommodityStateId;
            labSample.SampleCode = editedLabSample.SampleCode;
            //labSample.fkSamplingReasonId = editedLabSample.fkSamplingReasonId;
            labSample.fkSamplingTypeId = editedLabSample.fkSamplingTypeId;
            labSample.PremiseType = editedLabSample.PremiseType;
            labSample.PremiseName = editedLabSample.PremiseName;
            labSample.PremiseAddress = editedLabSample.PremiseAddress;
            labSample.SamplingDate = editedLabSample.SamplingDate;
            labSample.SamplingTime = editedLabSample.SamplingTime;
            labSample.Manufacturer = editedLabSample.Manufacturer;
            labSample.Distributor = editedLabSample.Distributor;
            labSample.Importer = editedLabSample.Importer;
            labSample.BrandName = editedLabSample.BrandName;
            labSample.fkSampleOriginId = editedLabSample.fkSampleOriginId;
            labSample.fkOriginCountryId = editedLabSample.fkOriginCountryId;
            labSample.fkPackagingTypeId = editedLabSample.fkPackagingTypeId;
            labSample.PackQuantitySize = editedLabSample.PackQuantitySize;
            labSample.BatchLotNumber = editedLabSample.BatchLotNumber;
            labSample.ShelfLife = editedLabSample.ShelfLife;
            labSample.StorageConditions = editedLabSample.StorageConditions;
            labSample.ContactName = editedLabSample.ContactName;
            labSample.ContactPhoneNumber = editedLabSample.ContactPhoneNumber;
            labSample.CollectedQuantity = editedLabSample.CollectedQuantity;
            labSample.fkSourceTypeId = editedLabSample.fkSourceTypeId;
            labSample.SourceName = editedLabSample.SourceName;
            labSample.SourceAddress = editedLabSample.SourceAddress;
            labSample.SourceContactName = editedLabSample.SourceContactName;
            labSample.SourceContactNumber = editedLabSample.SourceContactNumber;
            // labSample.Status = "Collected";
            db.Entry(labSample).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Debug.Write(e.InnerException.Message);
                return false;
            }
            return true;
        }

        public static List<CommodityState> GetAllCommodityStates(int? commodiyId)
        {
            return db.CommodityStates.Where(m => m.fkCommodityId == commodiyId).ToList();
        }
        public static List<Country> GetAllCountries()
        {
            return db.Countries.ToList();
        }
        public static List<PackagingType> GetAllPackagingTypes()
        {
            return db.PackagingTypes.ToList();
        }
        public static List<SampleOrigin> GetAllSampleOrigins()
        {
            return db.SampleOrigins.ToList();
        }
        public static List<SamplingReason> GetAllSamplingReasons()
        {
            return db.SamplingReasons.ToList();
        }
        public static List<SamplingType> GetAllSamplingTypes()
        {
            return db.SamplingTypes.ToList();
        }
        public static List<SamplingPlan> GetAllSamplingPlans()
        {
            db = new FoodSafetyDBEntities();
            return db.SamplingPlans.ToList();
        }

        public static List<string> GetAllPremiseTypes()
        {
            List<LabSample> labSamples = db.LabSamples.ToList();
            return GetDistinctPremiseTypesForSamples(labSamples);
        }

        public static SamplingPlan GetSamplingPlanFromId(int SamplingPlanId)
        {
            return db.SamplingPlans.Where(s => s.pkSamplingPlanId == SamplingPlanId).ToList().First();
        }

        public static bool UpdateSampleFromLabInfo(int id, LabSample editedLabSample)
        {
            LabSample labSample = GetLabSampleFromId(id);
            labSample.LabSampleNb = editedLabSample.LabSampleNb;
            labSample.Status = editedLabSample.Status;
            labSample.SampleQuantity = editedLabSample.SampleQuantity;
            if(editedLabSample.ReceivingDate.HasValue)
                labSample.ReceivingDate = editedLabSample.ReceivingDate.Value.Date;
            labSample.ReceivingTime = editedLabSample.ReceivingTime;
            labSample.Lab_Rejection_Comment = editedLabSample.Lab_Rejection_Comment;
            db.Entry(labSample).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Debug.Write(e.InnerException.Message);
                return false;
            }
            return true;
        }

        public static List<UOM> GetAllUOMs()
        {
            List<UOM> result = new List<UOM>();
            result = db.UOMs.ToList();
            return result;
        }

        //public static List<string> GetAllSamplingPlans()
        //{
        //    List<string> samplingPlans = new List<string>();
        //    List<LabSample> labSamples = db.LabSamples.ToList();
        //    if (labSamples != null && labSamples.Count > 0)
        //    {
        //        var distinctSamplingPlans = (from a in labSamples
        //                            select a.SamplingPlan.Trim()).Distinct().ToList();
        //        foreach (var plan in distinctSamplingPlans)
        //        {
        //            if (plan != null)
        //                samplingPlans.Add(plan);
        //        }
        //    }
        //    return samplingPlans;
        //}

        public static List<LabSample> getSamplesOfOneIdentity(string identityCode)
        {
            List<LabSample> result = new List<LabSample>();
            result = db.LabSamples.Where(s => s.SampleNb == identityCode).ToList();
            return result;
        }

        public static int getCountDistinctLabSamplesForCommodity(int commodityId)
        {
            List<LabSample> samples = db.LabSamples.Where(s => s.fkCommodityId == commodityId).ToList();
            var distinctSamples = (from a in samples
                                   select a.SampleNb.Trim()).Distinct().ToList();
            return distinctSamples.Count;
        }

        public static List<getLabSamplesBetweenDates_Result> GetLabSamplesBetweenDates(DateTime? fromDate, DateTime? toDate)
        {
            List<getLabSamplesBetweenDates_Result> result = db.getLabSamplesBetweenDates(fromDate, toDate).ToList();
            return result;
        }

        public static List<string> GetGeneratedSamplesForGovCazaUnassigned(string govCode, string cazaCode)
        {
            List<string> result = new List<string>();
            string prefix = govCode + cazaCode;
           // var samples = db.LabSamples.Where(s => s.SampleNb.StartsWith(prefix)).ToList();
            var unassignedSamples = (from sample in db.LabSamples
                                     join design in db.CollectionDesigns on sample.pkLabSampleId equals design.fkLabSampleId
                                     where sample.SampleNb.StartsWith(prefix) && design.fkCollectorId == null
                                     select sample.SampleNb);
            result = unassignedSamples.ToList();
            return result;
        }

        public static List<string> GetGeneratedSamplesForGovCazaPlanUnassigned(string govCode, string cazaCode, string planVal)
        {
            List<string> result = new List<string>();
            string prefix = planVal + govCode + cazaCode;
            // var samples = db.LabSamples.Where(s => s.SampleNb.StartsWith(prefix)).ToList();
            var unassignedSamples = (from sample in db.LabSamples
                                     join design in db.CollectionDesigns on sample.pkLabSampleId equals design.fkLabSampleId
                                     where sample.SampleNb.StartsWith(prefix) && design.fkCollectorId == null
                                     select sample.SampleNb);
            result = unassignedSamples.ToList();
            return result;
        }

        public static int? GetSamplingPlanIdFromText(string samplingPlan)
        {
            var temp = db.SamplingPlans.Where(s => s.SamplingPlanCode == samplingPlan).FirstOrDefault().pkSamplingPlanId;
            return temp;
        }
        public static SamplingPlan GetSamplingPlanFromText(string samplingPlan)
        {
            var temp = db.SamplingPlans.Where(s => s.SamplingPlanCode == samplingPlan).FirstOrDefault();
            return temp;
        }

        public static List<SourceType> GetAllSourceTypes()
        {
            List<SourceType> result = new List<SourceType>();
            return db.SourceTypes.ToList();
        }

        public static bool AddNewMRL(double MRL, int fkUOMId, int fkCommodityId, int fkPesticideResidueId)
        {
            PestResMRL pestResMRL = new PestResMRL();
            pestResMRL.MRL = MRL;
            pestResMRL.fkUOMId = fkUOMId;
            pestResMRL.fkCommodityId = fkCommodityId;
            pestResMRL.fkPesticideResidueId = fkPesticideResidueId;
            db.PestResMRLs.Add(pestResMRL);
            if (db.SaveChanges() > 0)
                return true;
            return false;
        }

        public static List<VetMRLModel> GetAllVetAndMRLs()
        {
            List<VetMRLModel> allVets_VM = new List<VetMRLModel>();
            var result = (from m in db.VetResMRLs
                          join vet in db.VeterinaryResidues.DefaultIfEmpty() on m.fkVetResidueID equals vet.pkVeterinaryResidueId
                          select new
                          {
                              pkVetMRLId = m.pkVetResMRLId,
                              pkVeterinaryResidueId = vet.pkVeterinaryResidueId,
                              VetResidueName = vet.VetResidueName,
                              AntibioticsClass = vet.AntibioticsClass,
                              mrl = m.MRL,
                              mrl_uom = m.UOM.unit,
                              commodityName = m.Commodity.CommodityName
                          }).ToList();

            foreach (var item in result)
            {
                VetMRLModel mRLVetVM = new VetMRLModel();
                //if (item.mrl_uom != 0)
                //    mRLViewModel.mrl_uom = GetUOMTextFromId(item.mrl_uom);

                mRLVetVM.pkVeterinaryResidueId = item.pkVeterinaryResidueId;
                mRLVetVM.VetResidueName = item.VetResidueName;
                mRLVetVM.mrl = item.mrl;
                mRLVetVM.mrl_uom = item.mrl_uom;
                mRLVetVM.AntibioticsClass = item.AntibioticsClass;
                mRLVetVM.commodityName = item.commodityName;
                mRLVetVM.pkVetMRLId = item.pkVetMRLId;
                allVets_VM.Add(mRLVetVM);
            }
            return allVets_VM;
        }

        public static VetMRLModel GetSingleVetAndMRL(int? id)
        {
            var result = (from m in db.VetResMRLs
                          join vet in db.VeterinaryResidues.DefaultIfEmpty() on m.fkVetResidueID equals vet.pkVeterinaryResidueId
                          where m.pkVetResMRLId == id
                          select new
                          {
                              pkVetMRLId = m.pkVetResMRLId,
                              pkVeterinaryResidueId = vet.pkVeterinaryResidueId,
                              VetResidueName = vet.VetResidueName,
                              AntibioticsClass = vet.AntibioticsClass,
                              mrl = m.MRL,
                              mrl_uom = m.UOM.unit,
                              commodityName = m.Commodity.CommodityName
                          }).FirstOrDefault();
                VetMRLModel mRLVetVM = new VetMRLModel();
                //if (item.mrl_uom != 0)
                //    mRLViewModel.mrl_uom = GetUOMTextFromId(item.mrl_uom);

                mRLVetVM.pkVeterinaryResidueId = result.pkVeterinaryResidueId;
                mRLVetVM.VetResidueName = result.VetResidueName;
                mRLVetVM.mrl = result.mrl;
                mRLVetVM.mrl_uom = result.mrl_uom;
                mRLVetVM.AntibioticsClass = result.AntibioticsClass;
                mRLVetVM.commodityName = result.commodityName;
                mRLVetVM.pkVetMRLId = result.pkVetMRLId;

            return mRLVetVM;
        }

        public static int GetMaxFromMPSamplingPlans()
        {
            var lastRecord = db.SamplingPlans.OrderByDescending(u => u.pkSamplingPlanId).FirstOrDefault();
            var lastMP_str = lastRecord.ShortCode.Remove(0, 2);
            int lastMP_int = Int32.Parse(lastMP_str);
            return lastMP_int;
        }
    }

    

}