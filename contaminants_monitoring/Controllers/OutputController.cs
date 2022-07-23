using Contaminants_Monitoring.Models;
using Contaminants_Monitoring.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Contaminants_Monitoring.Models.StatisticalAccess;

namespace Contaminants_Monitoring.Controllers
{
    public class OutputController : Controller
    {
        // GET: Output
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search()
        {
            SearchViewModel searchViewModel = new SearchViewModel();

            searchViewModel.commoditiesItems = initMultiSelectCommodities();
            searchViewModel.contaminantTypes = DataAccess.GetAllContaminantsTypes();
            searchViewModel.pesticideResiduesItems = initMultiSelectPesticides();
            searchViewModel.myCotoxinsItems = initMultiSelectMycotoxins();

            searchViewModel.years = initMultiSelectYears();

            searchViewModel.selectedPesticideResidues = new List<int?>();
            searchViewModel.selectedCommodities = new List<int?>();
            searchViewModel.selectedMycotoxins = new List<int?>();
            searchViewModel.selectedYears = new List<int?>();
            searchViewModel.monitoringPlans = DataAccess.GetAllSamplingPlans();

            searchViewModel.labSamples = new List<LabSample>();

            return View(searchViewModel);
        }

        [HttpPost]
        public ActionResult Search(SearchViewModel searchViewModel)
        {
            searchViewModel.commoditiesItems = initMultiSelectCommodities();
            searchViewModel.pesticideResiduesItems = initMultiSelectPesticides();
            searchViewModel.myCotoxinsItems = initMultiSelectMycotoxins();
            searchViewModel.years = initMultiSelectYears();
            searchViewModel.monitoringPlans = DataAccess.GetAllSamplingPlans();
            searchViewModel.contaminantTypes = DataAccess.GetAllContaminantsTypes();
            //bool boolAllComm = searchViewModel.allCommodities;
            //bool boolAllPRs = searchViewModel.allPesticidesResidues;
            //bool boolAllMycotoxins = searchViewModel.allMycotoxins;
            //bool boolAllYears = searchViewModel.allYears;
            List<LabSample> filteredResult = new List<LabSample>();
            if (searchViewModel.searchType == "PlanOption")
            {
                filteredResult = DataAccess.getAllLabSamples().Where(m => m.SamplingPlan == searchViewModel.fkMonitoringPlanId).ToList();
            }
            else
            {
                List<int?> selectedPestValues = new List<int?>();
                List<int?> selectedMyCotoxinsIds = new List<int?>();
                List<int?> selectedCommodities = new List<int?>();
                List<int?> selectedYears = new List<int?>();

                if (searchViewModel.selectedCommodities != null && searchViewModel.selectedCommodities.Contains(0))
                    selectedCommodities = null;
                //if (boolAllComm)
                //    selectedCommodities = null;
                else
                {
                    if (searchViewModel.selectedCommodities != null)
                        selectedCommodities = searchViewModel.selectedCommodities.ToList();
                }

                if (searchViewModel.selectedPesticideResidues != null && searchViewModel.selectedPesticideResidues.Contains(0))
                    selectedPestValues = null;
                else
                {
                    if (searchViewModel.selectedPesticideResidues != null)
                        selectedPestValues = searchViewModel.selectedPesticideResidues.ToList();
                }

                if (searchViewModel.selectedMycotoxins != null && searchViewModel.selectedMycotoxins.Contains(0))
                    selectedMyCotoxinsIds = null;
                else
                {
                    if (searchViewModel.selectedMycotoxins != null)
                        selectedMyCotoxinsIds = searchViewModel.selectedMycotoxins.ToList();
                }

                if (searchViewModel.selectedYears != null && searchViewModel.selectedYears.Contains(0))
                    selectedYears = null;
                else
                {
                    if (searchViewModel.selectedYears != null)
                        selectedYears = searchViewModel.selectedYears.ToList();
                }

                filteredResult = ApplyMainFilter(selectedPestValues, selectedMyCotoxinsIds, selectedCommodities, selectedYears);
            }
           

            if(searchViewModel.violateResultType == "ALL")
                searchViewModel.labSamples = filteredResult;
            else if (searchViewModel.violateResultType == "DET")
                searchViewModel.labSamples = DataAccess.RemoveUndetectedResult(filteredResult);
            else if (searchViewModel.violateResultType == "VIO")
                searchViewModel.labSamples = DataAccess.GetViolativeOnlyFromList(filteredResult);


            if (searchViewModel.separationVariable == "gov")
            {
                if (searchViewModel.showSummary)
                {
                    searchViewModel.mapOfTotalSamples_gov = StatisticalAccess.GetTotalOfSamples_Gov(searchViewModel.labSamples);
                    searchViewModel.mapOfLeftCensoredData_gov = StatisticalAccess.GetMapOfLeftProportions_Gov(searchViewModel.labSamples);
                    searchViewModel.mapOfStatFindings_gov = StatisticalAccess.GetMapOfFindingsStat_Gov(searchViewModel.labSamples);
                }
                if (searchViewModel.showDetailedFindings)
                {
                    searchViewModel.detailedFindings = new DetailedFindings
                    {
                        atLeastOneDetectedMoleculeList = GetCommodityGovernorateDictionaries_detected(searchViewModel.labSamples),
                        nonDetectedMoleculeList = GetCommodityGovernorateDictionaries_NotDetected(searchViewModel.labSamples),
                        quantifiedMoleculesList = GetCommodityGovernorateDictionaries_Quanitified(searchViewModel.labSamples),
                        nonQuantifiedMoleculeList = GetCommodityGovernorateDictionaries_NotQuanitified(searchViewModel.labSamples),
                        ViolatingcommConClassifcations_Map = GetGovernorateDictionaryList_Violating(searchViewModel.labSamples),
                        detectedPRForSpecificCommodity = GetGovernorateDictionaryList_totalPR(searchViewModel.labSamples),
                        maxDetectedPRInSamplePerCommodity = GetGovernorateDictionaryList_maxPR(searchViewModel.labSamples),
                        samplesCounterByMap = GetSamplesCounterDictionaryByGov(searchViewModel.labSamples),
                        ViolatingPerPRByMap = getViolatingPerPR_Gov(searchViewModel.labSamples),
                        moleculesDetectedRefSamples_Map = GetMapOfMoleculesPerSamplesByGov(searchViewModel.labSamples),
                        countPRatLeastOneSample_Map = countPRInatLeastOneSample_Gov(searchViewModel.labSamples)
                    };
                }
                
            }
            else if (searchViewModel.separationVariable == "org")
            {
                List<SampleOrigin> distinctSampleOrigins = DataAccess.GetDistinctOriginsForSamples(searchViewModel.labSamples);
                if (searchViewModel.showSummary)
                {
                    searchViewModel.mapOfTotalSamples_gov = StatisticalAccess.GetTotalOfSamples_Origin(searchViewModel.labSamples, distinctSampleOrigins);
                    searchViewModel.mapOfLeftCensoredData_gov = StatisticalAccess.GetMapOfLeftProportions_Origin(searchViewModel.labSamples, distinctSampleOrigins);
                    searchViewModel.mapOfStatFindings_gov = StatisticalAccess.GetMapOfFindingsStat_Origin(searchViewModel.labSamples, distinctSampleOrigins);
                }
                if (searchViewModel.showDetailedFindings)
                {
                    searchViewModel.detailedFindings = new DetailedFindings
                    {
                        atLeastOneDetectedMoleculeList = GetCommodityOriginDictionaries_detected(searchViewModel.labSamples, distinctSampleOrigins),
                        nonDetectedMoleculeList = GetCommodityOriginsDictionaries_NotDetected(searchViewModel.labSamples, distinctSampleOrigins),
                        quantifiedMoleculesList = GetCommodityOriginsDictionaries_Quanitified(searchViewModel.labSamples, distinctSampleOrigins),
                        nonQuantifiedMoleculeList = GetCommodityOriginDictionaries_NotQuanitified(searchViewModel.labSamples, distinctSampleOrigins),
                        ViolatingcommConClassifcations_Map = GetOriginsDictionaryList_Violating(searchViewModel.labSamples, distinctSampleOrigins),
                        detectedPRForSpecificCommodity = GetOriginsDictionaryList_totalPR(searchViewModel.labSamples, distinctSampleOrigins),
                        maxDetectedPRInSamplePerCommodity = GetOriginsDictionaryList_maxPR(searchViewModel.labSamples, distinctSampleOrigins),
                        samplesCounterByMap = GetSamplesCounterDictionaryByOrigin(searchViewModel.labSamples, distinctSampleOrigins),
                        ViolatingPerPRByMap = getViolatingPerPR_Origin(searchViewModel.labSamples, distinctSampleOrigins),
                        moleculesDetectedRefSamples_Map = GetMapOfMoleculesPerSamplesByOrigins(searchViewModel.labSamples, distinctSampleOrigins),
                        countPRatLeastOneSample_Map = countPRInatLeastOneSample_Origin(searchViewModel.labSamples, distinctSampleOrigins)
                    };
                }   
            }
            else if (searchViewModel.separationVariable == "prem")
            {
                List<string> distinctPremiseTypes = DataAccess.GetDistinctPremiseTypesForSamples(searchViewModel.labSamples);
                if (searchViewModel.showSummary)
                {
                    searchViewModel.mapOfTotalSamples_gov = StatisticalAccess.GetTotalOfSamples_PremiseType(searchViewModel.labSamples, distinctPremiseTypes);
                    searchViewModel.mapOfLeftCensoredData_gov = StatisticalAccess.GetMapOfLeftProportions_Premise(searchViewModel.labSamples, distinctPremiseTypes);
                    searchViewModel.mapOfStatFindings_gov = StatisticalAccess.GetMapOfFindingsStat_Premise(searchViewModel.labSamples, distinctPremiseTypes);
                }
                if (searchViewModel.showDetailedFindings)
                {
                    searchViewModel.detailedFindings = new DetailedFindings
                    {
                        atLeastOneDetectedMoleculeList = GetCommodityPremiseDictionaries_detected(searchViewModel.labSamples, distinctPremiseTypes),
                        nonDetectedMoleculeList = GetCommodityPremiseDictionaries_NotDetected(searchViewModel.labSamples, distinctPremiseTypes),
                        quantifiedMoleculesList = GetCommodityPremiseDictionaries_Quanitified(searchViewModel.labSamples, distinctPremiseTypes),
                        nonQuantifiedMoleculeList = GetCommodityPremiseDictionaries_NotQuanitified(searchViewModel.labSamples, distinctPremiseTypes),
                        ViolatingcommConClassifcations_Map = GetPremisesDictionaryList_Violating(searchViewModel.labSamples, distinctPremiseTypes),
                        detectedPRForSpecificCommodity = GetPremisesDictionaryList_totalPR(searchViewModel.labSamples, distinctPremiseTypes),
                        maxDetectedPRInSamplePerCommodity = GetPremisesDictionaryList_maxPR(searchViewModel.labSamples, distinctPremiseTypes),
                        samplesCounterByMap = GetSamplesCounterDictionaryByPremiseType(searchViewModel.labSamples, distinctPremiseTypes),
                        ViolatingPerPRByMap = getViolatingPerPR_PremiseType(searchViewModel.labSamples, distinctPremiseTypes),
                        moleculesDetectedRefSamples_Map = GetMapOfMoleculesPerSamplesByPremiseTypes(searchViewModel.labSamples, distinctPremiseTypes),
                        countPRatLeastOneSample_Map = countPRInatLeastOneSample_Premise(searchViewModel.labSamples, distinctPremiseTypes)
                    };
                }
                    
            }
            else
            {
                if (searchViewModel.showSummary)
                {
                    searchViewModel.totalNbOfSamples = StatisticalAccess.GetTotalOfSamples_All(searchViewModel.labSamples);
                    searchViewModel.totalNbOfResultSamples = searchViewModel.labSamples.Count;
                    searchViewModel.leftCensoredDataProportion = StatisticalAccess.GetProportionOfLeftCensoredData(searchViewModel.labSamples);
                    searchViewModel.findingsStats = StatisticalAccess.GetAllFindingStatRecords(searchViewModel.labSamples);
                }
                if (searchViewModel.showDetailedFindings)
                {
                    searchViewModel.detailedFindings = new DetailedFindings
                    {
                        atLeastOneDetectedMoleculeList = GetCommodityDictionaries_detected(searchViewModel.labSamples),
                        nonDetectedMoleculeList = GetCommodityDictionaries_NotDetected(searchViewModel.labSamples),
                        quantifiedMoleculesList = GetCommodityDictionaries_Quanitified(searchViewModel.labSamples),
                        nonQuantifiedMoleculeList = GetCommodityDictionaries_NotQuanitified(searchViewModel.labSamples),
                        commConClassifcations_violating = StatisticalAccess.GetNbOfSamples_ViolatingForCommCon(searchViewModel.labSamples),
                        commConClassifcations_unauthorized = StatisticalAccess.GetNbOfSamples_UnauthorizedForCommCon(searchViewModel.labSamples),
                        detectedPRForSpecificCommodity_all = StatisticalAccess.GetNbOfDetectedResiduesForCommodity(searchViewModel.labSamples),
                        detectedUnauthorizedForSpecificCommodity_all = StatisticalAccess.GetNbOfDetectedUnauthorizedForCommodity(searchViewModel.labSamples),
                        listUnauthorizedForSpecificCommodity_all = StatisticalAccess.GetListDetectedUnauthorizedForCommodity(searchViewModel.labSamples),
                        maxDetectedPRInSample = StatisticalAccess.GetMaxNbOfDetectedResidueForSpecificSampleNumber(searchViewModel.labSamples),
                        samplesCounter_all = GetSamplesCounterDictionary(searchViewModel.labSamples),
                        ViolatingPerPR_all = getViolatingPerPR_all(searchViewModel.labSamples),
                        pRMoleculesDetected = GetMapOfMoleculesPerSamples_all(searchViewModel.labSamples),
                        countPRsWithOneViolating = StatisticalAccess.GetNbOfMolecules_AtLeastOneViolatingSamplePerComm(searchViewModel.labSamples)
                    };
                }
                    
            }
            Session["LabSamples"] = searchViewModel.labSamples;
            return View(searchViewModel);
        }

        private static List<CommoditySeparatorDictionary> GetCommodityGovernorateDictionaries_detected(List<LabSample> labSamples)
        {
            List<Governorate> governorates = DataAccess.GetDistinctGovernoratesInSamples(labSamples);
            List<Commodity> commodities = DataAccess.GetDistinctCommoditiesInSamples(labSamples);
            List<CommoditySeparatorDictionary> commodityGovernorateDictionaries = new List<CommoditySeparatorDictionary>();
            foreach (var gov in governorates)
            {
                foreach (var com in commodities)
                {
                    commodityGovernorateDictionaries.Add(new CommoditySeparatorDictionary
                    {
                        commodity = com.CommodityName,
                        separator = gov.GovernorateName,
                        samplesNbForChemical = StatisticalAccess.AtLeastOneDetectedMoleculeForCommCon_Map(
                            labSamples.Where(s => s.fkCommodityId == com.pkCommodityId && s.fkGovernorateId == gov.pkGovernorateId).ToList())
                    });
                }
            }
            return commodityGovernorateDictionaries;
        }

        private static List<CommoditySeparatorDictionary> GetCommodityOriginDictionaries_detected(List<LabSample> labSamples, List<SampleOrigin> origins)
        {
            List<Commodity> commodities = DataAccess.GetDistinctCommoditiesInSamples(labSamples);
            List<CommoditySeparatorDictionary> commodityOriginsDictionaries = new List<CommoditySeparatorDictionary>();
            foreach (var org in origins)
            {
                foreach (var com in commodities)
                {
                    commodityOriginsDictionaries.Add(new CommoditySeparatorDictionary
                    {
                        commodity = com.CommodityName,
                        separator = org.OriginText,
                        samplesNbForChemical = StatisticalAccess.AtLeastOneDetectedMoleculeForCommCon_Map(
                            labSamples.Where(s => s.fkCommodityId == com.pkCommodityId && s.fkSampleOriginId == org.pkSampleOriginId).ToList())
                    });
                }
            }
            return commodityOriginsDictionaries;
        }

        private static List<CommoditySeparatorDictionary> GetCommodityPremiseDictionaries_detected(List<LabSample> labSamples, List<string> premiseTypes)
        {
            List<Commodity> commodities = DataAccess.GetDistinctCommoditiesInSamples(labSamples);
            List<CommoditySeparatorDictionary> commodityPremiseDictionaries = new List<CommoditySeparatorDictionary>();
            foreach (var pre in premiseTypes)
            {
                foreach (var com in commodities)
                {
                    commodityPremiseDictionaries.Add(new CommoditySeparatorDictionary
                    {
                        commodity = com.CommodityName,
                        separator = pre,
                        samplesNbForChemical = StatisticalAccess.AtLeastOneDetectedMoleculeForCommCon_Map(
                            labSamples.Where(s => s.fkCommodityId == com.pkCommodityId && s.PremiseType == pre).ToList())
                    });
                }
            }
            return commodityPremiseDictionaries;
        }

        private static List<CommoditySeparatorDictionary> GetCommodityDictionaries_detected(List<LabSample> labSamples)
        {
            List<Commodity> commodities = DataAccess.GetDistinctCommoditiesInSamples(labSamples);
            List<CommoditySeparatorDictionary> commodityDictionaries = new List<CommoditySeparatorDictionary>();
                foreach (var com in commodities)
                {
                commodityDictionaries.Add(new CommoditySeparatorDictionary
                    {
                        commodity = com.CommodityName,
                       // separator = pre,
                        samplesNbForChemical = StatisticalAccess.AtLeastOneDetectedMoleculeForCommCon_Map(
                            labSamples.Where(s => s.fkCommodityId == com.pkCommodityId).ToList())
                    });
                }
            return commodityDictionaries;
        }



        private static List<CommoditySeparatorDictionary> GetCommodityGovernorateDictionaries_NotDetected(List<LabSample> labSamples)
        {
            List<Governorate> governorates = DataAccess.GetDistinctGovernoratesInSamples(labSamples);
            List<Commodity> commodities = DataAccess.GetDistinctCommoditiesInSamples(labSamples);
            List<CommoditySeparatorDictionary> commodityGovernorateDictionaries = new List<CommoditySeparatorDictionary>();
            foreach (var gov in governorates)
            {
                foreach (var com in commodities)
                {
                    commodityGovernorateDictionaries.Add(new CommoditySeparatorDictionary
                    {
                        commodity = com.CommodityName,
                        separator = gov.GovernorateName,
                        samplesNbForChemical = StatisticalAccess.NoDetectedMoleculeForCommCon_Map(
                            labSamples.Where(s => s.fkCommodityId == com.pkCommodityId && s.fkGovernorateId == gov.pkGovernorateId).ToList())
                    });
                }
            }
            return commodityGovernorateDictionaries;
        }

        private static List<CommoditySeparatorDictionary> GetCommodityOriginsDictionaries_NotDetected(List<LabSample> labSamples, List<SampleOrigin> origins)
        {
            List<Commodity> commodities = DataAccess.GetDistinctCommoditiesInSamples(labSamples);
            List<CommoditySeparatorDictionary> commodityOriginsDictionaries = new List<CommoditySeparatorDictionary>();
            foreach (var org in origins)
            {
                foreach (var com in commodities)
                {
                    commodityOriginsDictionaries.Add(new CommoditySeparatorDictionary
                    {
                        commodity = com.CommodityName,
                        separator = org.OriginText,
                        samplesNbForChemical = StatisticalAccess.NoDetectedMoleculeForCommCon_Map(
                            labSamples.Where(s => s.fkCommodityId == com.pkCommodityId && s.fkSampleOriginId == org.pkSampleOriginId).ToList())
                    });
                }
            }
            return commodityOriginsDictionaries;
        }

        private static List<CommoditySeparatorDictionary> GetCommodityPremiseDictionaries_NotDetected(List<LabSample> labSamples, List<string> premiseTypes)
        {
            List<Commodity> commodities = DataAccess.GetDistinctCommoditiesInSamples(labSamples);
            List<CommoditySeparatorDictionary> commodityPremisesDictionaries = new List<CommoditySeparatorDictionary>();
            foreach (var pre in premiseTypes)
            {
                foreach (var com in commodities)
                {
                    commodityPremisesDictionaries.Add(new CommoditySeparatorDictionary
                    {
                        commodity = com.CommodityName,
                        separator = pre,
                        samplesNbForChemical = StatisticalAccess.NoDetectedMoleculeForCommCon_Map(
                            labSamples.Where(s => s.fkCommodityId == com.pkCommodityId && s.PremiseType == pre).ToList())
                    });
                }
            }
            return commodityPremisesDictionaries;
        }
        private static List<CommoditySeparatorDictionary> GetCommodityDictionaries_NotDetected(List<LabSample> labSamples)
        {
            List<Commodity> commodities = DataAccess.GetDistinctCommoditiesInSamples(labSamples);
            List<CommoditySeparatorDictionary> commodityDictionaries = new List<CommoditySeparatorDictionary>();
                foreach (var com in commodities)
                {
                    commodityDictionaries.Add(new CommoditySeparatorDictionary
                    {
                        commodity = com.CommodityName,
                        //separator = pre,
                        samplesNbForChemical = StatisticalAccess.NoDetectedMoleculeForCommCon_Map(
                            labSamples.Where(s => s.fkCommodityId == com.pkCommodityId).ToList())
                    });
                }
            return commodityDictionaries;
        }

        private static List<CommoditySeparatorDictionary> GetCommodityGovernorateDictionaries_Quanitified(List<LabSample> labSamples)
        {
            List<Governorate> governorates = DataAccess.GetDistinctGovernoratesInSamples(labSamples);
            List<Commodity> commodities = DataAccess.GetDistinctCommoditiesInSamples(labSamples);
            List<CommoditySeparatorDictionary> commodityGovernorateDictionaries = new List<CommoditySeparatorDictionary>();
            foreach (var gov in governorates)
            {
                foreach (var com in commodities)
                {
                    commodityGovernorateDictionaries.Add(new CommoditySeparatorDictionary
                    {
                        commodity = com.CommodityName,
                        separator = gov.GovernorateName,
                        samplesNbForChemical = StatisticalAccess.QuantifiedDetectionForCommCon(
                            labSamples.Where(s => s.fkCommodityId == com.pkCommodityId && s.fkGovernorateId == gov.pkGovernorateId).ToList())
                    });
                }
            }
            return commodityGovernorateDictionaries;
        }

        private static List<CommoditySeparatorDictionary> GetCommodityOriginsDictionaries_Quanitified(List<LabSample> labSamples, List<SampleOrigin> origins)
        {
            List<Commodity> commodities = DataAccess.GetDistinctCommoditiesInSamples(labSamples);
            List<CommoditySeparatorDictionary> commodityoriginsDictionaries = new List<CommoditySeparatorDictionary>();
            foreach (var org in origins)
            {
                foreach (var com in commodities)
                {
                    commodityoriginsDictionaries.Add(new CommoditySeparatorDictionary
                    {
                        commodity = com.CommodityName,
                        separator = org.OriginText,
                        samplesNbForChemical = StatisticalAccess.QuantifiedDetectionForCommCon(
                            labSamples.Where(s => s.fkCommodityId == com.pkCommodityId && s.fkSampleOriginId == org.pkSampleOriginId).ToList())
                    });
                }
            }
            return commodityoriginsDictionaries;
        }

        private static List<CommoditySeparatorDictionary> GetCommodityPremiseDictionaries_Quanitified(List<LabSample> labSamples, List<string> premiseTypes)
        {
            List<Commodity> commodities = DataAccess.GetDistinctCommoditiesInSamples(labSamples);
            List<CommoditySeparatorDictionary> commodityPremiseDictionaries = new List<CommoditySeparatorDictionary>();
            foreach (var pre in premiseTypes)
            {
                foreach (var com in commodities)
                {
                    commodityPremiseDictionaries.Add(new CommoditySeparatorDictionary
                    {
                        commodity = com.CommodityName,
                        separator = pre,
                        samplesNbForChemical = StatisticalAccess.QuantifiedDetectionForCommCon(
                            labSamples.Where(s => s.fkCommodityId == com.pkCommodityId && s.PremiseType == pre).ToList())
                    });
                }
            }
            return commodityPremiseDictionaries;
        }
        private static List<CommoditySeparatorDictionary> GetCommodityDictionaries_Quanitified(List<LabSample> labSamples)
        {
            List<Commodity> commodities = DataAccess.GetDistinctCommoditiesInSamples(labSamples);
            List<CommoditySeparatorDictionary> commodityDictionaries = new List<CommoditySeparatorDictionary>();
                foreach (var com in commodities)
                {
                commodityDictionaries.Add(new CommoditySeparatorDictionary
                    {
                        commodity = com.CommodityName,
                        samplesNbForChemical = StatisticalAccess.QuantifiedDetectionForCommCon(
                            labSamples.Where(s => s.fkCommodityId == com.pkCommodityId).ToList())
                    });
                }
            return commodityDictionaries;
        }

        private static List<CommoditySeparatorDictionary> GetCommodityGovernorateDictionaries_NotQuanitified(List<LabSample> labSamples)
        {
            List<Governorate> governorates = DataAccess.GetDistinctGovernoratesInSamples(labSamples);
            List<Commodity> commodities = DataAccess.GetDistinctCommoditiesInSamples(labSamples);
            List<CommoditySeparatorDictionary> commodityGovernorateDictionaries = new List<CommoditySeparatorDictionary>();
            foreach (var gov in governorates)
            {
                foreach (var com in commodities)
                {
                    commodityGovernorateDictionaries.Add(new CommoditySeparatorDictionary
                    {
                        commodity = com.CommodityName,
                        separator = gov.GovernorateName,
                        samplesNbForChemical = StatisticalAccess.NoQuantifiedDetectionForCommCon(
                            labSamples.Where(s => s.fkCommodityId == com.pkCommodityId && s.fkGovernorateId == gov.pkGovernorateId).ToList())
                    });
                }
            }
            return commodityGovernorateDictionaries;
        }
        private static List<CommoditySeparatorDictionary> GetCommodityOriginDictionaries_NotQuanitified(List<LabSample> labSamples, List<SampleOrigin> origins)
        {
            List<Commodity> commodities = DataAccess.GetDistinctCommoditiesInSamples(labSamples);
            List<CommoditySeparatorDictionary> commodityOriginsDictionaries = new List<CommoditySeparatorDictionary>();
            foreach (var org in origins)
            {
                foreach (var com in commodities)
                {
                    commodityOriginsDictionaries.Add(new CommoditySeparatorDictionary
                    {
                        commodity = com.CommodityName,
                        separator = org.OriginText,
                        samplesNbForChemical = StatisticalAccess.NoQuantifiedDetectionForCommCon(
                            labSamples.Where(s => s.fkCommodityId == com.pkCommodityId && s.fkSampleOriginId == org.pkSampleOriginId).ToList())
                    });
                }
            }
            return commodityOriginsDictionaries;
        }
        private static List<CommoditySeparatorDictionary> GetCommodityPremiseDictionaries_NotQuanitified(List<LabSample> labSamples, List<string> premises)
        {
            List<Commodity> commodities = DataAccess.GetDistinctCommoditiesInSamples(labSamples);
            List<CommoditySeparatorDictionary> commodityOriginsDictionaries = new List<CommoditySeparatorDictionary>();
            foreach (var pre in premises)
            {
                foreach (var com in commodities)
                {
                    commodityOriginsDictionaries.Add(new CommoditySeparatorDictionary
                    {
                        commodity = com.CommodityName,
                        separator = pre,
                        samplesNbForChemical = StatisticalAccess.NoQuantifiedDetectionForCommCon(
                            labSamples.Where(s => s.fkCommodityId == com.pkCommodityId && s.PremiseType == pre).ToList())
                    });
                }
            }
            return commodityOriginsDictionaries;
        }
        private static List<CommoditySeparatorDictionary> GetCommodityDictionaries_NotQuanitified(List<LabSample> labSamples)
        {
            List<Commodity> commodities = DataAccess.GetDistinctCommoditiesInSamples(labSamples);
            List<CommoditySeparatorDictionary> commodityDictionaries = new List<CommoditySeparatorDictionary>();
                foreach (var com in commodities)
                {
                commodityDictionaries.Add(new CommoditySeparatorDictionary
                    {
                        commodity = com.CommodityName,
                        samplesNbForChemical = StatisticalAccess.NoQuantifiedDetectionForCommCon(
                            labSamples.Where(s => s.fkCommodityId == com.pkCommodityId).ToList())
                    });
                }
            return commodityDictionaries;
        }


        private static Dictionary<string, List<CommConClassifcation>> GetGovernorateDictionaryList_Violating(List<LabSample> labSamples)
        {
            Dictionary<string, List<CommConClassifcation>> result = new Dictionary<string, List<CommConClassifcation>>();
            List<Governorate> governorates = DataAccess.GetDistinctGovernoratesInSamples(labSamples);
            foreach(Governorate gov in governorates)
            {
                List<CommConClassifcation> commConClassifcations = new List<CommConClassifcation>();
                commConClassifcations = StatisticalAccess.GetNbOfSamples_ViolatingForCommCon(labSamples.Where(l=>l.fkGovernorateId == gov.pkGovernorateId).ToList());
                result.Add(gov.GovernorateName, commConClassifcations);
            }
           // List<Commodity> commodities = DataAccess.GetDistinctCommoditiesInSamples(labSamples);
            // List<CommodityGovernorateDictionary> commodityGovernorateDictionaries = new List<CommodityGovernorateDictionary>();
            

            //foreach (var gov in governorates)
            //{
            //    foreach (var com in commodities)
            //    {
            //        commodityGovernorateDictionaries.Add(new CommodityGovernorateDictionary
            //        {
            //            commodity = com.CommodityName,
            //            governorate = gov.GovernorateName,
            //            samplesNbForChemical = StatisticalAccess.GetNbOfSamples_ViolatingForCommCon(
            //                labSamples.Where(s => s.fkCommodityId == com.pkCommodityId && s.fkGovernorateId == gov.pkGovernorateId).ToList())
            //        });
            //    }
            //}
            //return commodityGovernorateDictionaries;
            return result;
        }

        private static Dictionary<string, List<CommConClassifcation>> GetOriginsDictionaryList_Violating(List<LabSample> labSamples, List<SampleOrigin> origins)
        {
            Dictionary<string, List<CommConClassifcation>> result = new Dictionary<string, List<CommConClassifcation>>();
            foreach (SampleOrigin org in origins)
            {
                List<CommConClassifcation> commConClassifcations = new List<CommConClassifcation>();
                commConClassifcations = StatisticalAccess.GetNbOfSamples_ViolatingForCommCon(labSamples.Where(l => l.fkSampleOriginId == org.pkSampleOriginId).ToList());
                result.Add(org.OriginText, commConClassifcations);
            }
            return result;
        }
        private static Dictionary<string, List<CommConClassifcation>> GetPremisesDictionaryList_Violating(List<LabSample> labSamples, List<string> premiseTypes)
        {
            Dictionary<string, List<CommConClassifcation>> result = new Dictionary<string, List<CommConClassifcation>>();
            foreach (string pre in premiseTypes)
            {
                List<CommConClassifcation> commConClassifcations = new List<CommConClassifcation>();
                commConClassifcations = StatisticalAccess.GetNbOfSamples_ViolatingForCommCon(labSamples.Where(l => l.PremiseType == pre).ToList());
                result.Add(pre, commConClassifcations);
            }
           
            return result;
        }
        private static List<CommConClassifcation> GetDictionaryList_Violating(List<LabSample> labSamples)
        {
            List<CommConClassifcation> result = new List<CommConClassifcation>();
            result = StatisticalAccess.GetNbOfSamples_ViolatingForCommCon(labSamples).ToList();
            return result;
        }

        private static Dictionary<string, Dictionary<string, int>> GetGovernorateDictionaryList_totalPR(List<LabSample> labSamples)
        {
            Dictionary<string, Dictionary<string, int>> result = new Dictionary<string, Dictionary<string, int>>();
            List<Governorate> governorates = DataAccess.GetDistinctGovernoratesInSamples(labSamples);
            foreach (Governorate gov in governorates)
            {
                Dictionary<string, int> commDictionary = new Dictionary<string, int>();
                commDictionary = StatisticalAccess.GetNbOfDetectedResiduesForCommodity(labSamples.Where(l => l.fkGovernorateId == gov.pkGovernorateId).ToList());
                result.Add(gov.GovernorateName, commDictionary);
            }
            return result;
        }
        private static Dictionary<string, Dictionary<string, int>> GetOriginsDictionaryList_totalPR(List<LabSample> labSamples, List<SampleOrigin> origins)
        {
            Dictionary<string, Dictionary<string, int>> result = new Dictionary<string, Dictionary<string, int>>();
            foreach (SampleOrigin org in origins)
            {
                Dictionary<string, int> commDictionary = new Dictionary<string, int>();
                commDictionary = StatisticalAccess.GetNbOfDetectedResiduesForCommodity(labSamples.Where(l => l.fkSampleOriginId == org.pkSampleOriginId).ToList());
                result.Add(org.OriginText, commDictionary);
            }
            return result;
        }
        private static Dictionary<string, Dictionary<string, int>> GetPremisesDictionaryList_totalPR(List<LabSample> labSamples, List<string> premiseTypes)
        {
            Dictionary<string, Dictionary<string, int>> result = new Dictionary<string, Dictionary<string, int>>();
            foreach (string pre in premiseTypes)
            {
                Dictionary<string, int> commDictionary = new Dictionary<string, int>();
                commDictionary = StatisticalAccess.GetNbOfDetectedResiduesForCommodity(labSamples.Where(l => l.PremiseType == pre).ToList());
                result.Add(pre, commDictionary);
            }
            return result;
        }

        private static Dictionary<string, Dictionary<string, int>> GetGovernorateDictionaryList_maxPR(List<LabSample> labSamples)
        {
            Dictionary<string, Dictionary<string, int>> result = new Dictionary<string, Dictionary<string, int>>();
            List<Governorate> governorates = DataAccess.GetDistinctGovernoratesInSamples(labSamples);
            foreach (Governorate gov in governorates)
            {
                Dictionary<string, int> commDictionary = new Dictionary<string, int>();
                commDictionary = StatisticalAccess.GetMaxNbOfDetectedResidueForSpecificSampleNumber(labSamples.Where(l => l.fkGovernorateId == gov.pkGovernorateId).ToList());
                result.Add(gov.GovernorateName, commDictionary);
            }
            return result;
        }
        private static Dictionary<string, Dictionary<string, int>> GetOriginsDictionaryList_maxPR(List<LabSample> labSamples, List<SampleOrigin> origins)
        {
            Dictionary<string, Dictionary<string, int>> result = new Dictionary<string, Dictionary<string, int>>();
            foreach (SampleOrigin org in origins)
            {
                Dictionary<string, int> commDictionary = new Dictionary<string, int>();
                commDictionary = StatisticalAccess.GetMaxNbOfDetectedResidueForSpecificSampleNumber(labSamples.Where(l => l.fkSampleOriginId == org.pkSampleOriginId).ToList());
                result.Add(org.OriginText, commDictionary);
            }
            return result;
        }
        private static Dictionary<string, Dictionary<string, int>> GetPremisesDictionaryList_maxPR(List<LabSample> labSamples, List<string> premiseTypes)
        {
            Dictionary<string, Dictionary<string, int>> result = new Dictionary<string, Dictionary<string, int>>();
            foreach (string pre in premiseTypes)
            {
                Dictionary<string, int> commDictionary = new Dictionary<string, int>();
                commDictionary = StatisticalAccess.GetMaxNbOfDetectedResidueForSpecificSampleNumber(labSamples.Where(l => l.PremiseType == pre).ToList());
                result.Add(pre, commDictionary);
            }
            return result;
        }

        public Dictionary<string, List<SamplesCounter>> GetSamplesCounterDictionaryByGov(List<LabSample> labSamples)
        {
            Dictionary<string, List<SamplesCounter>> result = new Dictionary<string, List<SamplesCounter>>();
            List<Governorate> governorates = DataAccess.GetDistinctGovernoratesInSamples(labSamples);
            List<Commodity> distinctCommodites = DataAccess.GetDistinctCommoditiesInSamples(labSamples);
            foreach (Governorate gov in governorates)
            {
                List<SamplesCounter> samplesCounters = new List<SamplesCounter>();
                foreach(Commodity commodity in distinctCommodites)
                {
                    SamplesCounter counter = new SamplesCounter();
                    counter.commodity = commodity.CommodityName;
                    List<LabSample> specificLabSamples = new List<LabSample>();
                    specificLabSamples = labSamples.Where(s => s.fkGovernorateId == gov.pkGovernorateId && s.fkCommodityId == commodity.pkCommodityId).ToList();
                    if (specificLabSamples.Count > 0)
                    {
                        counter.OnePROnly = StatisticalAccess.GetNbOfSamples_OneResidueOnlyPerCommodity(specificLabSamples);
                        counter.twoPROnly = StatisticalAccess.GetNbOfSamples_TwoResiduePerCommodity(specificLabSamples);
                        counter.threeToTenPR = StatisticalAccess.GetNbOfSamples_ThreeToTenResiduePerCommodity(specificLabSamples);
                        counter.TenToMaxPR = StatisticalAccess.GetNbOfSamples_TenToMaxResiduePerCommodity(specificLabSamples);
                        int total = specificLabSamples.Count;
                        counter.OnePROnly_percent = counter.OnePROnly * 100 / total;
                        counter.twoPROnly_percent = counter.twoPROnly * 100 / total;
                        counter.threeToTenPR = counter.threeToTenPR * 100 / total;
                        counter.TenToMaxPR_percent = counter.TenToMaxPR_percent * 100 / total;
                        samplesCounters.Add(counter);
                    }
                }
                result.Add(gov.GovernorateName, samplesCounters);

            }
                return result;
        }
        public Dictionary<string, List<SamplesCounter>> GetSamplesCounterDictionaryByOrigin(List<LabSample> labSamples, List<SampleOrigin> origins)
        {
            Dictionary<string, List<SamplesCounter>> result = new Dictionary<string, List<SamplesCounter>>();
            List<Commodity> distinctCommodites = DataAccess.GetDistinctCommoditiesInSamples(labSamples);
            foreach (SampleOrigin org in origins)
            {
                List<SamplesCounter> samplesCounters = new List<SamplesCounter>();
                foreach (Commodity commodity in distinctCommodites)
                {
                    SamplesCounter counter = new SamplesCounter();
                    counter.commodity = commodity.CommodityName;
                    List<LabSample> specificLabSamples = new List<LabSample>();
                    specificLabSamples = labSamples.Where(s => s.fkSampleOriginId == org.pkSampleOriginId && s.fkCommodityId == commodity.pkCommodityId).ToList();
                    if (specificLabSamples.Count > 0)
                    {
                        counter.OnePROnly = StatisticalAccess.GetNbOfSamples_OneResidueOnlyPerCommodity(specificLabSamples);
                        counter.twoPROnly = StatisticalAccess.GetNbOfSamples_TwoResiduePerCommodity(specificLabSamples);
                        counter.threeToTenPR = StatisticalAccess.GetNbOfSamples_ThreeToTenResiduePerCommodity(specificLabSamples);
                        counter.TenToMaxPR = StatisticalAccess.GetNbOfSamples_TenToMaxResiduePerCommodity(specificLabSamples);
                        int total = specificLabSamples.Count;
                        counter.OnePROnly_percent = counter.OnePROnly * 100 / total;
                        counter.twoPROnly_percent = counter.twoPROnly * 100 / total;
                        counter.threeToTenPR = counter.threeToTenPR * 100 / total;
                        counter.TenToMaxPR_percent = counter.TenToMaxPR_percent * 100 / total;
                        samplesCounters.Add(counter);
                    }
                }
                result.Add(org.OriginText, samplesCounters);

            }
            return result;
        }
        public Dictionary<string, List<SamplesCounter>> GetSamplesCounterDictionaryByPremiseType(List<LabSample> labSamples, List<string> premiseTypes)
        {
            Dictionary<string, List<SamplesCounter>> result = new Dictionary<string, List<SamplesCounter>>();
            List<Commodity> distinctCommodites = DataAccess.GetDistinctCommoditiesInSamples(labSamples);
            foreach (string pre in premiseTypes)
            {
                List<SamplesCounter> samplesCounters = new List<SamplesCounter>();
                foreach (Commodity commodity in distinctCommodites)
                {
                    SamplesCounter counter = new SamplesCounter();
                    counter.commodity = commodity.CommodityName;
                    List<LabSample> specificLabSamples = new List<LabSample>();
                    specificLabSamples = labSamples.Where(s => s.PremiseType == pre && s.fkCommodityId == commodity.pkCommodityId).ToList();
                    if (specificLabSamples.Count > 0)
                    {
                        counter.OnePROnly = StatisticalAccess.GetNbOfSamples_OneResidueOnlyPerCommodity(specificLabSamples);
                        counter.twoPROnly = StatisticalAccess.GetNbOfSamples_TwoResiduePerCommodity(specificLabSamples);
                        counter.threeToTenPR = StatisticalAccess.GetNbOfSamples_ThreeToTenResiduePerCommodity(specificLabSamples);
                        counter.TenToMaxPR = StatisticalAccess.GetNbOfSamples_TenToMaxResiduePerCommodity(specificLabSamples);
                        int total = specificLabSamples.Count;
                        counter.OnePROnly_percent = counter.OnePROnly * 100 / total;
                        counter.twoPROnly_percent = counter.twoPROnly * 100 / total;
                        counter.threeToTenPR = counter.threeToTenPR * 100 / total;
                        counter.TenToMaxPR_percent = counter.TenToMaxPR_percent * 100 / total;
                        samplesCounters.Add(counter);
                    }
                }
                result.Add(pre, samplesCounters);

            }
            return result;
        }
        public List<SamplesCounter> GetSamplesCounterDictionary(List<LabSample> labSamples)
        {
            List<SamplesCounter> result = new List<SamplesCounter>();
            List<Commodity> distinctCommodites = DataAccess.GetDistinctCommoditiesInSamples(labSamples);
            foreach (Commodity commodity in distinctCommodites)
             {
                    SamplesCounter counter = new SamplesCounter();
                    counter.commodity = commodity.CommodityName;
                    List<LabSample> specificLabSamples = new List<LabSample>();
                    specificLabSamples = labSamples.Where(s => s.fkCommodityId == commodity.pkCommodityId).ToList();
                    if (specificLabSamples.Count > 0)
                    {
                        counter.OnePROnly = StatisticalAccess.GetNbOfSamples_OneResidueOnlyPerCommodity(specificLabSamples);
                        counter.twoPROnly = StatisticalAccess.GetNbOfSamples_TwoResiduePerCommodity(specificLabSamples);
                        counter.threeToTenPR = StatisticalAccess.GetNbOfSamples_ThreeToTenResiduePerCommodity(specificLabSamples);
                        counter.TenToMaxPR = StatisticalAccess.GetNbOfSamples_TenToMaxResiduePerCommodity(specificLabSamples);
                        int total = specificLabSamples.Count;
                        counter.OnePROnly_percent = Math.Round((double)counter.OnePROnly * 100 / total, 2);
                        counter.twoPROnly_percent = Math.Round((double)counter.twoPROnly * 100 / total, 2);
                        counter.threeToTenPR_percent = Math.Round((double)counter.threeToTenPR * 100 / total, 2);
                        counter.TenToMaxPR_percent = Math.Round((double)counter.TenToMaxPR_percent * 100 / total, 2);
                    result.Add(counter);
                    }
                }
            return result;
        }
        public static Dictionary<string, Dictionary<string, NumberPercentageVM>> getViolatingPerPR_Gov(List<LabSample> labSamples)
        {
            Dictionary<string, Dictionary<string, NumberPercentageVM>> result = new Dictionary<string, Dictionary<string, NumberPercentageVM>>();
            List<Governorate> governorates = DataAccess.GetDistinctGovernoratesInSamples(labSamples);
            foreach (Governorate gov in governorates)
            {
                List<LabSample> govLabSamples = new List<LabSample>();
                govLabSamples = labSamples.Where(s => s.fkGovernorateId == gov.pkGovernorateId).ToList();
                if(govLabSamples.Count > 0)
                {
                    Dictionary<string, NumberPercentageVM> PRResult = new Dictionary<string, NumberPercentageVM>();
                    PRResult = StatisticalAccess.GetNbOfSamples_ViolatingPerMoleculePerCommodity(govLabSamples);
                    result.Add(gov.GovernorateName, PRResult);
                }
                
             }
                return result;
        }
        public static Dictionary<string, Dictionary<string, NumberPercentageVM>> getViolatingPerPR_Origin(List<LabSample> labSamples, List<SampleOrigin> origins)
        {
            Dictionary<string, Dictionary<string, NumberPercentageVM>> result = new Dictionary<string, Dictionary<string, NumberPercentageVM>>();
            foreach (SampleOrigin org in origins)
            {
                List<LabSample> orgLabSamples = new List<LabSample>();
                orgLabSamples = labSamples.Where(s => s.fkSampleOriginId == org.pkSampleOriginId).ToList();
                if (orgLabSamples.Count > 0)
                {
                    Dictionary<string, NumberPercentageVM> PRResult = new Dictionary<string, NumberPercentageVM>();
                    PRResult = StatisticalAccess.GetNbOfSamples_ViolatingPerMoleculePerCommodity(orgLabSamples);
                    result.Add(org.OriginText, PRResult);
                }

            }
            return result;
        }
        public static Dictionary<string, Dictionary<string, NumberPercentageVM>> getViolatingPerPR_PremiseType(List<LabSample> labSamples, List<string> premiseTypes)
        {
            Dictionary<string, Dictionary<string, NumberPercentageVM>> result = new Dictionary<string, Dictionary<string, NumberPercentageVM>>();
            foreach (string pre in premiseTypes)
            {
                List<LabSample> preLabSamples = new List<LabSample>();
                preLabSamples = labSamples.Where(s => s.PremiseType == pre).ToList();
                if (preLabSamples.Count > 0)
                {
                    Dictionary<string, NumberPercentageVM> PRResult = new Dictionary<string, NumberPercentageVM>();
                    PRResult = StatisticalAccess.GetNbOfSamples_ViolatingPerMoleculePerCommodity(preLabSamples);
                    result.Add(pre, PRResult);
                }

            }
            return result;
        }
        public static Dictionary<string, NumberPercentageVM> getViolatingPerPR_all(List<LabSample> labSamples)
        {
            Dictionary<string, NumberPercentageVM> result = new Dictionary<string, NumberPercentageVM>();
            if (labSamples.Count > 0)
            {
                result = StatisticalAccess.GetNbOfSamples_ViolatingPerMoleculePerCommodity(labSamples);
            }
            return result;
        }

        public static Dictionary<string, PRMoleculesDetected> GetMapOfMoleculesPerSamplesByGov(List<LabSample> labSamples)
        {
            Dictionary<string, PRMoleculesDetected> result = new Dictionary<string, PRMoleculesDetected>();
            List<Governorate> governorates = DataAccess.GetDistinctGovernoratesInSamples(labSamples);
            foreach (Governorate gov in governorates)
            {
                PRMoleculesDetected pRMoleculesDetected = new PRMoleculesDetected();
                List<LabSample> labSamples_gov = labSamples.Where(l=>l.fkGovernorateId == gov.pkGovernorateId).ToList();
                pRMoleculesDetected.molecules_1sample = StatisticalAccess.GetNbOfMolecules_DetectedInOneSampleOnlyPerComm(labSamples_gov);
                pRMoleculesDetected.molecules_2to10sample = StatisticalAccess.GetNbOfMolecules_DetectedIn2To10SamplesPerComm(labSamples_gov);
                pRMoleculesDetected.molecules_11to50sample = StatisticalAccess.GetNbOfMolecules_DetectedIn11To50SamplesPerComm(labSamples_gov);
                pRMoleculesDetected.molecules_51to100sample = StatisticalAccess.GetNbOfMolecules_DetectedIn51To100SamplesPerComm(labSamples_gov);
                pRMoleculesDetected.molecules_101to1000sample = StatisticalAccess.GetNbOfMolecules_DetectedIn101To1000SamplesPerComm(labSamples_gov);
                pRMoleculesDetected.molecules_more1000sample = StatisticalAccess.GetNbOfMolecules_DetectedInMoreThan1000SamplesPerComm(labSamples_gov);
                result.Add(gov.GovernorateName, pRMoleculesDetected);
            }
                return result;
        }
        public static Dictionary<string, PRMoleculesDetected> GetMapOfMoleculesPerSamplesByOrigins(List<LabSample> labSamples, List<SampleOrigin> origins)
        {
            Dictionary<string, PRMoleculesDetected> result = new Dictionary<string, PRMoleculesDetected>();
            foreach (SampleOrigin org in origins)
            {
                PRMoleculesDetected pRMoleculesDetected = new PRMoleculesDetected();
                List<LabSample> labSamples_org = labSamples.Where(l => l.fkSampleOriginId == org.pkSampleOriginId).ToList();
                pRMoleculesDetected.molecules_1sample = StatisticalAccess.GetNbOfMolecules_DetectedInOneSampleOnlyPerComm(labSamples_org);
                pRMoleculesDetected.molecules_2to10sample = StatisticalAccess.GetNbOfMolecules_DetectedIn2To10SamplesPerComm(labSamples_org);
                pRMoleculesDetected.molecules_11to50sample = StatisticalAccess.GetNbOfMolecules_DetectedIn11To50SamplesPerComm(labSamples_org);
                pRMoleculesDetected.molecules_51to100sample = StatisticalAccess.GetNbOfMolecules_DetectedIn51To100SamplesPerComm(labSamples_org);
                pRMoleculesDetected.molecules_101to1000sample = StatisticalAccess.GetNbOfMolecules_DetectedIn101To1000SamplesPerComm(labSamples_org);
                pRMoleculesDetected.molecules_more1000sample = StatisticalAccess.GetNbOfMolecules_DetectedInMoreThan1000SamplesPerComm(labSamples_org);
                result.Add(org.OriginText, pRMoleculesDetected);
            }
            return result;
        }
        public static Dictionary<string, PRMoleculesDetected> GetMapOfMoleculesPerSamplesByPremiseTypes(List<LabSample> labSamples, List<string> premiseTypes)
        {
            Dictionary<string, PRMoleculesDetected> result = new Dictionary<string, PRMoleculesDetected>();
            foreach (string pre in premiseTypes)
            {
                PRMoleculesDetected pRMoleculesDetected = new PRMoleculesDetected();
                List<LabSample> labSamples_pre = labSamples.Where(l => l.PremiseType == pre).ToList();
                pRMoleculesDetected.molecules_1sample = StatisticalAccess.GetNbOfMolecules_DetectedInOneSampleOnlyPerComm(labSamples_pre);
                pRMoleculesDetected.molecules_2to10sample = StatisticalAccess.GetNbOfMolecules_DetectedIn2To10SamplesPerComm(labSamples_pre);
                pRMoleculesDetected.molecules_11to50sample = StatisticalAccess.GetNbOfMolecules_DetectedIn11To50SamplesPerComm(labSamples_pre);
                pRMoleculesDetected.molecules_51to100sample = StatisticalAccess.GetNbOfMolecules_DetectedIn51To100SamplesPerComm(labSamples_pre);
                pRMoleculesDetected.molecules_101to1000sample = StatisticalAccess.GetNbOfMolecules_DetectedIn101To1000SamplesPerComm(labSamples_pre);
                pRMoleculesDetected.molecules_more1000sample = StatisticalAccess.GetNbOfMolecules_DetectedInMoreThan1000SamplesPerComm(labSamples_pre);
                result.Add(pre, pRMoleculesDetected);
            }
            return result;
        }
        public static PRMoleculesDetected GetMapOfMoleculesPerSamples_all(List<LabSample> labSamples)
        {
            PRMoleculesDetected pRMoleculesDetected = new PRMoleculesDetected();
            pRMoleculesDetected.molecules_1sample = StatisticalAccess.GetNbOfMolecules_DetectedInOneSampleOnlyPerComm(labSamples);
            pRMoleculesDetected.molecules_2to10sample = StatisticalAccess.GetNbOfMolecules_DetectedIn2To10SamplesPerComm(labSamples);
            pRMoleculesDetected.molecules_11to50sample = StatisticalAccess.GetNbOfMolecules_DetectedIn11To50SamplesPerComm(labSamples);
            pRMoleculesDetected.molecules_51to100sample = StatisticalAccess.GetNbOfMolecules_DetectedIn51To100SamplesPerComm(labSamples);
            pRMoleculesDetected.molecules_101to1000sample = StatisticalAccess.GetNbOfMolecules_DetectedIn101To1000SamplesPerComm(labSamples);
            pRMoleculesDetected.molecules_more1000sample = StatisticalAccess.GetNbOfMolecules_DetectedInMoreThan1000SamplesPerComm(labSamples);
            return pRMoleculesDetected;
        }

        public static Dictionary<string, int> countPRInatLeastOneSample_Gov(List<LabSample> labSamples)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            List<Governorate> governorates = DataAccess.GetDistinctGovernoratesInSamples(labSamples);
            foreach (Governorate gov in governorates)
            {
                List<LabSample> samples_gov = new List<LabSample>();
                samples_gov = labSamples.Where(s => s.fkGovernorateId == gov.pkGovernorateId).ToList();
                int count = StatisticalAccess.GetNbOfMolecules_AtLeastOneViolatingSamplePerComm(samples_gov);
                result.Add(gov.GovernorateName, count);
            }
                return result;
        }
        public static Dictionary<string, int> countPRInatLeastOneSample_Origin(List<LabSample> labSamples, List<SampleOrigin> origins)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            foreach (SampleOrigin org in origins)
            {
                List<LabSample> samples_org = new List<LabSample>();
                samples_org = labSamples.Where(s => s.fkSampleOriginId == org.pkSampleOriginId).ToList();
                int count = StatisticalAccess.GetNbOfMolecules_AtLeastOneViolatingSamplePerComm(samples_org);
                result.Add(org.OriginText, count);
            }
            return result;
        }
        public static Dictionary<string, int> countPRInatLeastOneSample_Premise(List<LabSample> labSamples, List<string> premiseTypes)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            foreach (string pre in premiseTypes)
            {
                List<LabSample> samples_pre = new List<LabSample>();
                samples_pre = labSamples.Where(s => s.PremiseType == pre).ToList();
                int count = StatisticalAccess.GetNbOfMolecules_AtLeastOneViolatingSamplePerComm(samples_pre);
                result.Add(pre, count);
            }
            return result;
        }

        private List<LabSample> ApplyMainFilter(List<int?> selectedPestValues, List<int?> selectedMycotoxinsIds, List<int?> selectedCommodities, List<int?> years)
        {
            List<LabSample> result = new List<LabSample>();

            result = DataAccess.getAllLabSamples();
            if (selectedCommodities != null && selectedCommodities.Count > 0)
            {
                result.Clear();
                result = DataAccess.FilterSamplesByCommodities(selectedCommodities);
            }
            if (years != null && years.Count > 0)
            {
                result = DataAccess.filterSamplesByYears(years, result);
            }
            if (selectedPestValues != null && selectedPestValues.Count > 0)
            {
                result = DataAccess.filterSamplesByPesticides(selectedPestValues, result);
                if (selectedMycotoxinsIds != null && selectedMycotoxinsIds.Count > 0)
                {
                    result.AddRange(DataAccess.filterSamplesByMycotoxin(selectedMycotoxinsIds, result));
                }
            }
            else
            {
                if (selectedMycotoxinsIds != null && selectedMycotoxinsIds.Count > 0)
                {
                    result = DataAccess.filterSamplesByMycotoxin(selectedMycotoxinsIds, result);
                }
            }
            


            return result;
        }

        public ActionResult CommodityHistory()
        {
            CommodityHistoryVM commodityHistoryVM = new CommodityHistoryVM();
            commodityHistoryVM.commodityHistoryItems = DataAccess.GetCommodityHistory();
            return View(commodityHistoryVM);
        }

        public ActionResult ContaminantHistory()
        {
            ContaminantHistoryVM contaminantHistoryVM = new ContaminantHistoryVM();
            contaminantHistoryVM.contaminantHistoryItems = DataAccess.GetContaminantHistory();
            return View(contaminantHistoryVM);
        }

        public ActionResult PrintContaminantHistory()
        {
            //  return Redirect("~/Reports/ComplaintFormality.aspx?id=" + id);
            return Redirect("~/Printouts/ContaminantsHistory.aspx");
        }


        private List<SelectListItem> initMultiSelectPesticides()
        {
            List<PesticideResidue> pesticideResidues = DataAccess.getAllPesticideResidues();

            List<SelectListItem> pesticidesSelectList = new List<SelectListItem>();
            SelectListItem All_item = new SelectListItem
            {
                Text = "All",
                Value = "0"
            };
            pesticidesSelectList.Add(All_item);
            foreach (PesticideResidue pesticide in pesticideResidues)
            {
                SelectListItem item = new SelectListItem();
                item.Text = pesticide.PestResName;
                item.Value = pesticide.pkPesticideResidueId.ToString();
                pesticidesSelectList.Add(item);
            }
            return pesticidesSelectList;
        }

        private List<SelectListItem> initMultiSelectMycotoxins()
        {
            List<Mycotoxin> mycotoxins = DataAccess.getAllMycotoxins();
            List<SelectListItem> myCotoxinsSelectList = new List<SelectListItem>();
            SelectListItem All_item = new SelectListItem
            {
                Text = "All",
                Value = "0"
            };
            myCotoxinsSelectList.Add(All_item);
            foreach (Mycotoxin mycotoxin in mycotoxins)
            {
                SelectListItem item = new SelectListItem
                {
                    Text = mycotoxin.MycotoxinName,
                    Value = mycotoxin.pkMycotoxinId.ToString()
                };
                myCotoxinsSelectList.Add(item);
            }
            return myCotoxinsSelectList;
        }

        

        private List<SelectListItem> initMultiSelectCommodities()
        {
            List<Commodity> commodities = DataAccess.getAllCommodities();
            List<SelectListItem> commoditiesSelectList = new List<SelectListItem>();

            SelectListItem All_item = new SelectListItem
            {
                Text = "All",
                Value = "0"
            };
            commoditiesSelectList.Add(All_item);

            foreach (Commodity commodity in commodities)
            {
                SelectListItem item = new SelectListItem
                {
                    Text = commodity.CommodityCode + " - " + commodity.CommodityName,
                    Value = commodity.pkCommodityId.ToString()
                };
                commoditiesSelectList.Add(item);
            }
            return commoditiesSelectList;
        }

        private List<SelectListItem> initMultiSelectYears()
        {
            List<int> years = new List<int>();
            for (int i = 2000; i < 2100; i++)
            {
                years.Add(i);
            }

            List<SelectListItem> yearsSelectList = new List<SelectListItem>();
            SelectListItem All_item = new SelectListItem
            {
                Text = "All",
                Value = "0"
            };
            yearsSelectList.Add(All_item);
            foreach (int y in years)
            {
                SelectListItem item = new SelectListItem();
                item.Text = y.ToString();
                item.Value = y.ToString();
                yearsSelectList.Add(item);
            }
            return yearsSelectList;
        }


        [HttpGet]
        public ActionResult RiskToolFile()
        {
            return View();
        }

        public ActionResult DownloadRiskToolFile()
        {
            string filename = "Risk assessment excel tool.xlsx";
            string filepath = Server.MapPath("~/App_Data/Templates/") + filename;
            // string filepath = AppDomain.CurrentDomain.BaseDirectory + "/Path/To/File/" + filename;
            byte[] filedata = System.IO.File.ReadAllBytes(filepath);
            string contentType = MimeMapping.GetMimeMapping(filepath);

            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = filename,
                Inline = true,
            };

            Response.AppendHeader("Content-Disposition", cd.ToString());

            return File(filedata, contentType);
        }

        //[HttpPost]
        //public ActionResult DisplayContaminantMolecules(string val)
        //{
        //    if (!string.IsNullOrEmpty(val))
        //    {
        //        Laboratory selectedLab = DataAccess. (int.Parse(val));

        //        return Json(new
        //        {
        //            Success = "true",
        //            Data = new
        //            {
        //                list = selectedLab.LaboratoryName,
        //            }
        //        });
        //    }
        //    return Json(new { Success = "false" });
        //} 

        //public ActionResult ExportToExcel()
        //{
        //    return RedirectToAction();
        //}
    }
}