using Contaminants_Monitoring.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Contaminants_Monitoring.Models.StatisticalAccess;

namespace Contaminants_Monitoring.ViewModels
{
    public class SearchViewModel
    {
        public List<LabSample> labSamples { get; set; }

        public IEnumerable<SelectListItem> commoditiesItems { get; set; }

        public IEnumerable<int?> selectedCommodities { get; set; }

        public IEnumerable<SelectListItem> pesticideResiduesItems { get; set; }

        public IEnumerable<int?> selectedPesticideResidues { get; set; }

        public IEnumerable<SelectListItem> myCotoxinsItems { get; set; }

        public IEnumerable<int?> selectedMycotoxins { get; set; }

        public IEnumerable<SelectListItem> ConOrganismsItems { get; set; }

        public IEnumerable<int?> selectedContOrganisms { get; set; }

        public IEnumerable<SelectListItem> years { get; set; }
        public IEnumerable<int?> selectedYears { get; set; }

        public IEnumerable<SamplingPlan> monitoringPlans { get; set; }
        public int fkMonitoringPlanId { get; set; }

        public IEnumerable<ContaminantType> contaminantTypes { get; set; }
        public int fkContaminantTypeId { get; set; }

        public bool allCommodities { get; set; }

        public bool allPesticidesResidues { get; set; }

        public bool allMycotoxins { get; set; }
        public bool allYears { get; set; }
        public string searchType { get; set; }
        //public bool allResults { get; set; }
        //public bool detectedOnlyResults { get; set; }
        //public bool ViolativeOnlyResults { get; set; }

        public string violateResultType { get; set; }

        public string separationVariable { get; set; }

        public List<FindingsStat> findingsStats { get; set; }
        public double leftCensoredDataProportion { get; set; }
        public int totalNbOfSamples { get; set; }

        public int totalNbOfResultSamples { get; set; }

        public DetailedFindings detailedFindings { get; set; }


        //Maps of Groupings
        public Dictionary<string, int> mapOfTotalSamples_gov { get; set; }
        public Dictionary<string, double> mapOfLeftCensoredData_gov { get; set; }
        public Dictionary<string, List<FindingsStat>> mapOfStatFindings_gov { get; set; }
        public Dictionary<string, DetailedFindings> mapOfDetailedFindings_gov { get; set; }

        public bool showSummary { get; set; }
        public bool showDetailedFindings { get; set; }



    }
    public class DetailedFindings
    {
        //Percentage and number of samples with contaminant/residue detection 
        //(at least one detected molecule in a sample per chemical type per commodity) (Conc final >LOD)
        public int AtLeastOneDetectedMoleculeInSamples { get; set; }
        public double AtLeastOneDetectedMoleculeInSamples_percent { get; set; }

        //this variable may be used if no separateion requested
        public Dictionary<string, NumberPercentageVM> AtLeastOneDetectedMolecule_Map { get; set; }

        //new Lists
        public List<CommoditySeparatorDictionary> atLeastOneDetectedMoleculeList { get; set; }
        public List<CommoditySeparatorDictionary> nonDetectedMoleculeList { get; set; }
        public List<CommoditySeparatorDictionary> quantifiedMoleculesList { get; set; }
        public List<CommoditySeparatorDictionary> nonQuantifiedMoleculeList { get; set; }


        //Percentage and number of samples with no contaminant/residue detection
        //    (zero detected molecule in a sample per chemical type per commodity) (Conc final < LOD)
        public int noDetectedMoleculeInSamples { get; set; }
        public double noDetectedMoleculeInSamples_percent { get; set; }

        //Percentage and number of samples with contaminant/residue quantification
        //    (at least one quantified molecule in a sample per chemical type per commodity) (Conc final > LOQ)
        public int quantifiedSamplesCount { get; set; }
        public double quantifiedSamplesCount_percent { get; set; }

        //Percentage and number of samples with no contaminant/residue quantification
        //(zero quantified molecule in a sample per chemical type per commodity) (LOD<Conc final<LOQ)
        public int notQuantifiedSamplesCount { get; set; }
        public double notQuantifiedSamplesCount_percent { get; set; }

        //Percentage and number of total violating samples (Conc final > ML/MRL) per contaminant/residue and per commodity
        //public int violatingSamplesCount { get; set; }
        //public double violatingSamplesCount_percent { get; set; }
        public List<CommConClassifcation> commConClassifcations_violating { get; set; }
        public Dictionary<string, List<CommConClassifcation>> ViolatingcommConClassifcations_Map { get; set; }

        //Total number of detected residues in a commodity(only for PR)
        //public int detectedPRForSpecificCommodity { get; set; }
        //public Dictionary<string, int> detectedPRForSpecificCommodity { get; set; }
        public Dictionary<string, Dictionary<string, int>> detectedPRForSpecificCommodity { get; set; }
        public Dictionary<string, int> detectedPRForSpecificCommodity_all { get; set; }

        public Dictionary<string, Dictionary<string, int>> maxDetectedPRInSamplePerCommodity { get; set; }

        //Maximum number “Max” of detected residue in one sample at a time per commodity(only for PR)
        //public int maxDetectedPRInSample { get; set; }
        public Dictionary<string, int> maxDetectedPRInSample { get; set; }

        public Dictionary<string, List<SamplesCounter>> samplesCounterByMap { get; set; }
        public List<SamplesCounter> samplesCounter_all { get; set; }
        public Dictionary<string, Dictionary<string, NumberPercentageVM>> ViolatingPerPRByMap { get; set; }
        public Dictionary<string, NumberPercentageVM> ViolatingPerPR_all { get; set; }
        public Dictionary<string,PRMoleculesDetected> moleculesDetectedRefSamples_Map { get; set; }

        public PRMoleculesDetected pRMoleculesDetected { get; set; }

        public Dictionary<string, int> countPRatLeastOneSample_Map { get; set; }

        //Percentage and number of samples with one residue only per commodity(only for PR)
        public int samplesWithOnePRCount { get; set; }
        public double samplesWithOnePRCount_percent { get; set; }

        //Percentage and number of samples with two residue per commodity(only for PR)
        public int samplesWithTwoPRCount { get; set; }
        public double samplesWithTwoPRCount_percent { get; set; }

        //Percentage and number of samples with three to 10 residues per commodity(only for PR)
        public int samplesWith3to10PRCount { get; set; }
        public double samplesWith3to10PRCount_percent { get; set; }

        //Percentage and number of samples with 10 to “Max” residues per commodity(only for PR)
        public int samplesWith10toMaxPRCount { get; set; }
        public double samplesWith10toMaxPRCount_percent { get; set; }

        //Percentage and number of samples with residue detection for each pesticide molecule(per commodity) (only for PR)
        //public int countSamplesOfSpecificPR { get; set; }
        //public double countSamplesOfSpecificPR_percent { get; set; }
        public Dictionary<string, int> countSamplesOfSpecificPR { get; set; }

        //Percentage and number of violating samples per pesticide molecule per commodity(Conc final > MRL) (only for PR)
        //public int countViolatingSamplesPerPR { get; set; }
        //public double countViolatingSamplesPerPR_percent { get; set; }
        public Dictionary<string, int> countViolatingSamplesPerPR { get; set; }

        //Percentage and number of pesticide molecules detected in one sample only per commodity(only for PR)
        public int countPRDetectedIn1sample { get; set; }
        public double countPRDetectedIn1sample_percent { get; set; }

        //Percentage and number of pesticide molecules detected in two to ten samples per commodity(only for PR)
        public int countPRDetectedIn2to10sample { get; set; }
        public double countPRDetectedIn2to10sample_percent { get; set; }

        //Percentage and number of pesticide molecules detected in eleven to 50 samples per commodity(only for PR)
        public int countPRDetectedIn11to50sample { get; set; }
        public double countPRDetectedIn11to50sample_percent { get; set; }

        //Percentage and number of pesticide molecules detected in 51 to 100 samples per commodity(only for PR)
        public int countPRDetectedIn51to100sample { get; set; }
        public double countPRDetectedIn51to100sample_percent { get; set; }

        //Percentage and number of pesticide molecules detected in 101 to 1000 samples per commodity(only for PR)
        public int countPRDetectedIn101to1000sample { get; set; }
        public double countPRDetectedIn101to1000sample_percent { get; set; }

        //Percentage and number of pesticide molecules detected in more than 1000 samples per commodity(only for PR)
        public int countPRDetectedInMorethan1000sample { get; set; }
        public double countPRDetectedInInMorethan1000sample_percent { get; set; }

        //Number of pesticide molecules with at least one sample per commodity exceeding MRL(Conc final > MRL) (only for PR)
        public int countPRsWithOneViolating { get; set; }

        public List<CommConClassifcation> commConClassifcations_unauthorized { get; set; }

        public Dictionary<string, int> detectedUnauthorizedForSpecificCommodity_all { get; set; }
        public Dictionary<string, List<string>> listUnauthorizedForSpecificCommodity_all { get; set; }


    }
}