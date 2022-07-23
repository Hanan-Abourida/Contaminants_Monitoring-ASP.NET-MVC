using Contaminants_Monitoring.ViewModels;
using MathNet.Numerics.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Contaminants_Monitoring.Models
{
    public class StatisticalAccess
    {
        public static FoodSafetyDBEntities db = new FoodSafetyDBEntities();

        private static List<Governorate> GetDistinctGovernoratesForSamples(List<LabSample> labSamples)
        {
            List<Governorate> result = new List<Governorate>();
            if (labSamples != null && labSamples.Count > 0)
            {
                var governorates = (from a in labSamples
                                  select a.fkGovernorateId).Distinct().ToList();
                foreach (var govId in governorates)
                {
                    if (govId != null)
                        result.Add(db.Governorates.Where(g => g.pkGovernorateId == govId).FirstOrDefault());
                }
            }
            return result;
        }

        public static List<Mycotoxin> GetDistinctMycotoxinsForFindings(List<LabSample> labSamples)
        {
            List<Mycotoxin> result = new List<Mycotoxin>();
            if (labSamples != null && labSamples.Count > 0)
            {
                var myCotoxins = (from a in labSamples
                                  select a.fkMycotoxinId).Distinct().ToList();
                foreach (var mycotoxinId in myCotoxins)
                {
                    if(mycotoxinId != null)
                        result.Add(db.Mycotoxins.Where(c => c.pkMycotoxinId == mycotoxinId).FirstOrDefault());
                }
            }
            return result;
        }

        public static List<PesticideResidue> GetDistinctResiduesForFindings(List<LabSample> labSamples)
        {
            List<PesticideResidue> result = new List<PesticideResidue>();
            if (labSamples != null && labSamples.Count > 0)
            {
                var PRs = (from a in labSamples
                                  select a.fkPesticideResidueId).Distinct().ToList();
                foreach (var residueId in PRs)
                {
                    if(residueId != null)
                        result.Add(db.PesticideResidues.Where(p => p.pkPesticideResidueId == residueId).FirstOrDefault());
                }
            }
            return result;
        }

        public static List<PesticideResidue> GetDistinctUnauthorizedResiduesForFindings(List<LabSample> labSamples)
        {
            List<PesticideResidue> result = new List<PesticideResidue>();
            if (labSamples != null && labSamples.Count > 0)
            {
                var PRs = (from a in labSamples
                           select a.fkPesticideResidueId).Distinct().ToList();
                foreach (var residueId in PRs)
                {
                    if (residueId != null)
                        result.Add(db.PesticideResidues.Where(p => p.pkPesticideResidueId == residueId && p.Banned == true).FirstOrDefault());
                }
            }
            return result;
        }

        public static List<PesticideResidue> GetDistinctDetectedUnauthorizedResiduesForFindings(List<LabSample> labSamples)
        {
            List<PesticideResidue> result = new List<PesticideResidue>();
            if (labSamples != null && labSamples.Count > 0)
            {
                var PRs = (from a in labSamples
                           where a.ConFinal != "ND" && a.ConFinal != "NQ"
                           select a.fkPesticideResidueId).Distinct().ToList();
                foreach (var residueId in PRs)
                {
                    if (residueId != null)
                    {
                        PesticideResidue pr = new PesticideResidue();
                        pr = db.PesticideResidues.Where(p => p.pkPesticideResidueId == residueId && p.Banned != null && p.Banned == true).FirstOrDefault();
                        if(pr != null)
                            result.Add(pr);
                    }
                        
                }
            }
            return result;
        }

        public static List<Commodity> GetDistinctCommoditiesForFindings(List<LabSample> labSamples)
        {
            List<Commodity> result = new List<Commodity>();
            if (labSamples != null && labSamples.Count > 0)
            {
                var commodities = (from a in labSamples
                           select a.fkCommodityId).Distinct().ToList();
                foreach (var commId in commodities)
                {
                    result.Add(db.Commodities.Where(p => p.pkCommodityId == commId).FirstOrDefault());
                }
            }
            return result;
        }

        public static List<double> GetConFinalsForSingleMycotoxinFinding(List<LabSample> labSamples, int commodityId, int MycotoxinId)
        {
            var conFinals = (from a in labSamples
                             where a.fkCommodityId == commodityId && a.fkMycotoxinId == MycotoxinId
                             select a.ConFinal);
            List<double> conFinals_double = new List<double>();
            foreach (var con in conFinals)
            {
                if(con.Trim() != "ND" && con.Trim() != "NQ")
                    conFinals_double.Add(Double.Parse(con));
            }
            return conFinals_double;
        }

        public static List<double> GetConFinalsForSinglePRFinding(List<LabSample> labSamples, int commodityId, int pesticideResidueId)
        {
            var conFinals = (from a in labSamples
                             where a.fkCommodityId == commodityId && a.fkPesticideResidueId == pesticideResidueId
                             select a.ConFinal);
            List<double> conFinals_double = new List<double>();
            foreach (var con in conFinals)
            {
                if (!String.IsNullOrEmpty(con) && con != "ND" && con != "NQ")
                    conFinals_double.Add(Double.Parse(con));
            }
            return conFinals_double;
        }

        public static List<FindingsStat> GetAllFindingsStatForMycotoxins(List<Commodity> commodities, List<Mycotoxin> myCotoxins, List<LabSample> labSamples)
        {
            List<FindingsStat> result = new List<FindingsStat>();
            foreach(Commodity commodity in commodities)
            {
                foreach (Mycotoxin mycotoxin in myCotoxins)
                {
                    FindingsStat findingsStat = new FindingsStat();
                    findingsStat.commodity = commodity.CommodityCode + " - " + commodity.CommodityName;
                    findingsStat.molecule = mycotoxin.MycotoxinName;
                    List<double> conFinals = GetConFinalsForSingleMycotoxinFinding(labSamples, commodity.pkCommodityId, mycotoxin.pkMycotoxinId);
                    findingsStat.mean = Statistics.Mean(conFinals);
                    findingsStat.standardDeviation = Statistics.StandardDeviation(conFinals);
                    findingsStat.minimum = Statistics.Minimum(conFinals);
                    findingsStat.maximum = Statistics.Maximum(conFinals);
                    //findingsStat.range = Statistics.InterquartileRange(conFinals);
                    findingsStat.range = findingsStat.maximum - findingsStat.minimum;
                    findingsStat.median = Statistics.Median(conFinals);
                    findingsStat.quantile1 = Statistics.Quantile(conFinals, 1);
                    findingsStat.quantile2 = Statistics.Quantile(conFinals, 2);
                    findingsStat.quantile3 = Statistics.Quantile(conFinals, 3);
                    findingsStat.quantile4 = Statistics.Quantile(conFinals, 4);
                    //findingsStat.quantile90 = Statistics.Quantile(conFinals, 90);
                    //findingsStat.quantile95 = Statistics.Quantile(conFinals, 95);

                    findingsStat.percentile5 = Statistics.Percentile(conFinals, 5);
                    findingsStat.percentile25 = Statistics.Percentile(conFinals, 25);
                    findingsStat.percentile75 = Statistics.Percentile(conFinals, 75);
                    findingsStat.percentile90 = Statistics.Percentile(conFinals, 90);
                    findingsStat.percentile95 = Statistics.Percentile(conFinals, 95);
                    findingsStat.coefficientOfVariation = findingsStat.standardDeviation * 100 / findingsStat.mean;
                } 
            }
            return result;
        }

        public static List<FindingsStat> GetAllFindingsStatForPesticideResidue(List<Commodity> commodities, List<PesticideResidue> pesticideResidues, List<LabSample> labSamples)
        {
            if (labSamples == null || labSamples.Count == 0)
                return null;
            List<FindingsStat> result = new List<FindingsStat>();
            foreach (Commodity commodity in commodities)
            {
                foreach (PesticideResidue pesticideResidue in pesticideResidues)
                {
                    FindingsStat findingsStat = new FindingsStat();
                    findingsStat.commodity = commodity.CommodityCode + " - " + commodity.CommodityName;
                    findingsStat.molecule = pesticideResidue.PestResName;
                    List<double> conFinals = GetConFinalsForSinglePRFinding(labSamples, commodity.pkCommodityId, pesticideResidue.pkPesticideResidueId);
                    findingsStat.mean = Statistics.Mean(conFinals);
                    findingsStat.standardDeviation = Statistics.StandardDeviation(conFinals);
                    findingsStat.minimum = Statistics.Minimum(conFinals);
                    findingsStat.maximum = Statistics.Maximum(conFinals);
                    // findingsStat.range = Statistics.InterquartileRange(conFinals);
                    findingsStat.range = findingsStat.maximum - findingsStat.minimum;
                    findingsStat.median = Statistics.Median(conFinals);
                    findingsStat.quantile1 = Statistics.Quantile(conFinals, 1);
                    findingsStat.quantile2 = Statistics.Quantile(conFinals, 2);
                    findingsStat.quantile3 = Statistics.Quantile(conFinals, 3);
                    findingsStat.quantile4 = Statistics.Quantile(conFinals, 4);

                    findingsStat.percentile5 = Statistics.Percentile(conFinals, 5);
                    findingsStat.percentile25 = Statistics.Percentile(conFinals, 25);
                    findingsStat.percentile75 = Statistics.Percentile(conFinals, 75);
                    findingsStat.percentile90 = Statistics.Percentile(conFinals, 90);
                    findingsStat.percentile95 = Statistics.Percentile(conFinals, 95);
                    findingsStat.coefficientOfVariation = findingsStat.standardDeviation * 100 / findingsStat.mean;
                    result.Add(findingsStat);
                }
            }
            return result;
        }


        public static List<FindingsStat> GetAllFindingStatRecords(List<LabSample> labSamples)
        {
            List<FindingsStat> result = new List<FindingsStat>();
            if (labSamples != null && labSamples.Count > 0)
            {
                List<Mycotoxin> distinctMycotoxins = GetDistinctMycotoxinsForFindings(labSamples);
                List<PesticideResidue> distinctPesticideResidues = GetDistinctResiduesForFindings(labSamples);
                List<Commodity> distinctCommodities = GetDistinctCommoditiesForFindings(labSamples);

                result = GetAllFindingsStatForMycotoxins(distinctCommodities, distinctMycotoxins, labSamples);
                result.AddRange(GetAllFindingsStatForPesticideResidue(distinctCommodities, distinctPesticideResidues, labSamples));
                //If new Contaminant added to the system, must add code here GetAllFindingsStatForNewContaminant
            }

            return result;
        }

        public static double GetProportionOfLeftCensoredData(List<LabSample> labSamples)
        {
            double resultPercentage = 0;
            List<LabSample> leftCensoredData = new List<LabSample>();
            foreach (LabSample labSample in labSamples)
            {
                if(labSample.ConFinal == "NQ" || labSample.ConFinal == "ND")
                {
                    leftCensoredData.Add(labSample);
                }
                //double conFinal = 0;
                //Double.TryParse(labSample.ConFinal, out conFinal);
                //if(conFinal < labSample.LOQ)
                //{
                //    leftCensoredData.Add(labSample);
                //}

            }
            if(labSamples.Count > 0) resultPercentage = leftCensoredData.Count() * 100 / labSamples.Count();
            return resultPercentage;
        }

        //1-Percentage and number of samples with contaminant/residue detection 
        //(at least one detected molecule in a sample per chemical type per commodity) (Conc final >LOD)
        public static int GetNbOfSamples_AtLeastOneDetectedMoleculeForCommCon(List<LabSample> labSamples)
        {
            var distinctSamples = (from a in labSamples
                                                select a.SampleNb).Distinct().ToList();
            //samples With At least One Detected Molecule In Its Records 
            List<string> detected = new List<string>(); 
            foreach(string sampleNb in distinctSamples)
            {
                List<LabSample> recordsOfOneSample = labSamples.Where(s => s.SampleNb == sampleNb).ToList();
                bool detectedFlag = false;
                foreach (LabSample sample in recordsOfOneSample) 
                {
                    double conFinalDouble = 0;
                    if (double.TryParse(sample.ConFinal, out conFinalDouble))
                    {
                        detectedFlag = true;
                    }
                    //if (sample.ConFinal == "NQ")
                    //    detectedFlag = true;
                    //else if (sample.ConFinal != "ND" && double.TryParse(sample.ConFinal, out conFinalDouble))
                    //{
                    //    if (sample.LOD != null && conFinalDouble > sample.LOD)
                    //        detectedFlag = true;
                    //    else if (sample.LOD == null) detectedFlag = true;
                    //}
                }
                if(detectedFlag) detected.Add(sampleNb);
            }
            return detected.Count;
        }

        //1-Percentage and number of samples with contaminant/residue detection 
        //(at least one detected molecule in a sample per chemical type per commodity) (Conc final >LOD)
        public static Dictionary<string, NumberPercentageVM> AtLeastOneDetectedMoleculeForCommCon_Map(List<LabSample> labSamples)
        {
            //the parameter labsamples is assumed here to be related to one commodity and one separationValue. i.e: one governorate
            var distinctSamples = (from a in labSamples
                                   select a.SampleNb).Distinct().ToList();
            Dictionary<string, NumberPercentageVM> result = new Dictionary<string, NumberPercentageVM>();

            //populate map with count 0
            result.Add("Pesticide Residue", new NumberPercentageVM(0,0));
            result.Add("Mycotoxin", new NumberPercentageVM(0, 0));

            //samples With At least One Detected Molecule In Its Records 
            foreach (string sampleNb in distinctSamples)
            {
                List<LabSample> recordsOfOneSample = labSamples.Where(s => s.SampleNb == sampleNb).ToList();
                bool detectedFlag = false;
                LabSample currentSample = new LabSample();
                foreach (LabSample sample in recordsOfOneSample)
                {
                    currentSample = sample;
                    double conFinalDouble = 0;
                    if (double.TryParse(sample.ConFinal, out conFinalDouble))
                    {
                        detectedFlag = true;
                    }

                    //currentSample = sample;
                    //double conFinalDouble = 0;
                    //if (sample.ConFinal == "NQ" || sample.ConFinal == "ND")
                    //    detectedFlag = false;
                    //else if (double.TryParse(sample.ConFinal, out conFinalDouble))
                    //{
                    //    if (sample.LOD != null && conFinalDouble > sample.LOD)
                    //        detectedFlag = true;
                    //}
                }
                if (detectedFlag)
                {
                    //increment count in dictionary
                    string chemicalName = db.ContaminantTypes.Where(c => c.pkContaminantTypeId == currentSample.fkContaminantTypeId).First().ContaminantName;
                    foreach(var value in result.Keys)
                    {
                        if (value == chemicalName)
                        {
                            result[value].number++;
                            int totalCount = labSamples.Where(l => l.fkContaminantTypeId == currentSample.fkContaminantTypeId).ToList().Count;
                            if(totalCount > 0) result[value].percentage = result[value].number * 100 / totalCount;
                        }
                    }
                }
            }
            return result;
        }


        //2-Percentage and number of samples with no contaminant/residue detection
        //    (zero detected molecule in a sample per chemical type per commodity) (Conc final < LOD)
        public static int GetNbOfSamples_NoDetectedMoleculeForCommCon(List<LabSample> labSamples)
        {
            var distinctSamples = (from a in labSamples
                                   select a.SampleNb).Distinct().ToList();
            //samples With no Detected Molecule In Its Records 
            List<string> notDetected = new List<string>();
            foreach (string sampleNb in distinctSamples)
            {
                List<LabSample> recordsOfOneSample = labSamples.Where(s => s.SampleNb == sampleNb).ToList();
                bool detectedFlag = false;
                foreach (LabSample sample in recordsOfOneSample)
                {
                  //  double conFinalDouble = 0;
                    if (sample.ConFinal == "ND")
                    {
                        detectedFlag = false;
                    }
                    else detectedFlag = true;
                    //if (sample.ConFinal == "NQ")
                    //    detectedFlag = true;
                    //else if (sample.ConFinal != "ND" && double.TryParse(sample.ConFinal, out conFinalDouble))
                    //{
                    //    if (sample.LOD != null && conFinalDouble > sample.LOD)
                    //        detectedFlag = true;
                    //    else if (sample.LOD == null) detectedFlag = true;
                    //}
                }
                if (!detectedFlag) notDetected.Add(sampleNb);
            }
            return notDetected.Count;
        }

        //2-Percentage and number of samples with no contaminant/residue detection
        //    (zero detected molecule in a sample per chemical type per commodity) (Conc final < LOD)
        public static Dictionary<string, NumberPercentageVM> NoDetectedMoleculeForCommCon_Map(List<LabSample> labSamples)
        {
            var distinctSamples = (from a in labSamples
                                   select a.SampleNb).Distinct().ToList();
            Dictionary<string, NumberPercentageVM> result = new Dictionary<string, NumberPercentageVM>();
            //populate map with count 0
            result.Add("Pesticide Residue", new NumberPercentageVM(0, 0));
            result.Add("Mycotoxin", new NumberPercentageVM(0, 0));
            //samples With no Detected Molecule In Its Records 
            // List<string> notDetected = new List<string>();
            LabSample currentSample = new LabSample();
            foreach (string sampleNb in distinctSamples)
            {
                List<LabSample> recordsOfOneSample = labSamples.Where(s => s.SampleNb == sampleNb).ToList();
                bool detectedFlag = false;
                foreach (LabSample sample in recordsOfOneSample)
                {
                    currentSample = sample; 
                  //  double conFinalDouble = 0;
                    if (sample.ConFinal == "ND")
                    {
                        detectedFlag = false;
                    }
                    else detectedFlag = true;
                    //if (sample.ConFinal == "NQ" || sample.ConFinal == "ND")
                    //    detectedFlag = false;
                    //else if (sample.ConFinal != "ND" && double.TryParse(sample.ConFinal, out conFinalDouble))
                    //{
                    //    if (sample.LOD != null && conFinalDouble > sample.LOD)
                    //        detectedFlag = true;
                    //    else if(sample.LOD != null && conFinalDouble < sample.LOD) detectedFlag = false;
                    //}
                }
                if (!detectedFlag)
                {
                    //increment count in dictionary
                    string chemicalName = db.ContaminantTypes.Where(c => c.pkContaminantTypeId == currentSample.fkContaminantTypeId).First().ContaminantName;
                    foreach (var value in result.Keys)
                    {
                        if (value == chemicalName)
                        {
                            result[value].number++;
                            int totalCount = labSamples.Where(l => l.fkContaminantTypeId == currentSample.fkContaminantTypeId).ToList().Count;
                            if (totalCount > 0) result[value].percentage = result[value].number * 100 / totalCount;
                        }
                    }
                    //notDetected.Add(sampleNb);
                }
            }
            return result;
        }

        //3-Percentage and number of samples with contaminant/residue quantification
        //    (at least one quantified molecule in a sample per chemical type per commodity) (Conc final > LOQ)
        public static int GetNbOfSamples_QuantifiedDetectionForCommCon(List<LabSample> labSamples)
        {
            var distinctSamples = (from a in labSamples
                                   select a.SampleNb).Distinct().ToList();
            List<string> quantifiedSamples = new List<string>();
            foreach (string sampleNb in distinctSamples)
            {
                List<LabSample> recordsOfOneSample = labSamples.Where(s => s.SampleNb == sampleNb).ToList();
                bool quantified = false;
                foreach (LabSample sample in recordsOfOneSample)
                {
                    double conFinal_double = 0;
                    if(sample.ConFinal != "NQ" && sample.ConFinal != "ND" && Double.TryParse(sample.ConFinal, out conFinal_double))
                    {
                        quantified = true;
                    }
                }
                if (quantified) quantifiedSamples.Add(sampleNb);
            }

            return quantifiedSamples.Count;
        }

        //3-Percentage and number of samples with contaminant/residue quantification
        //    (at least one quantified molecule in a sample per chemical type per commodity) (Conc final > LOQ)
        public static Dictionary<string, NumberPercentageVM> QuantifiedDetectionForCommCon(List<LabSample> labSamples)
        {
            var distinctSamples = (from a in labSamples
                                   select a.SampleNb).Distinct().ToList();
            Dictionary<string, NumberPercentageVM> result = new Dictionary<string, NumberPercentageVM>();
            LabSample currentSample = new LabSample();
            foreach (string sampleNb in distinctSamples)
            {
                List<LabSample> recordsOfOneSample = labSamples.Where(s => s.SampleNb == sampleNb).ToList();
                bool quantified = false;
                foreach (LabSample sample in recordsOfOneSample)
                {
                    currentSample = sample;
                    double conFinal_double = 0;
                    if (sample.ConFinal != "NQ" && sample.ConFinal != "ND" && Double.TryParse(sample.ConFinal, out conFinal_double))
                    {
                        if (sample.LOQ != null && conFinal_double > sample.LOQ)
                            quantified = true;

                    }
                }
                if (quantified)
                {
                    //increment count in dictionary
                    string chemicalName = db.ContaminantTypes.Where(c => c.pkContaminantTypeId == currentSample.fkContaminantTypeId).First().ContaminantName;
                    if(result.Keys !=null && !result.ContainsKey(chemicalName))
                    {
                        int totalCount = labSamples.Where(l => l.fkContaminantTypeId == currentSample.fkContaminantTypeId).ToList().Count;
                        NumberPercentageVM numberPercentageVM = new NumberPercentageVM(1, 1*100/totalCount);
                        result.Add(chemicalName, numberPercentageVM);
                    }
                    else foreach (var value in result.Keys)
                    {
                        if (value == chemicalName)
                        {
                            result[value].number++;
                            int totalCount = labSamples.Where(l => l.fkContaminantTypeId == currentSample.fkContaminantTypeId).ToList().Count;
                            if (totalCount > 0) result[value].percentage = result[value].number * 100 / totalCount;
                        }
                    }
                }
            }
            return result;
        }

        //4-Percentage and number of samples with no contaminant/residue quantification
        //(zero quantified molecule in a sample per chemical type per commodity) (LOD<Conc final<LOQ)
        public static Dictionary<string, NumberPercentageVM> NoQuantifiedDetectionForCommCon(List<LabSample> labSamples)
        {
            var distinctSamples = (from a in labSamples
                                   select a.SampleNb).Distinct().ToList();
            Dictionary<string, NumberPercentageVM> result = new Dictionary<string, NumberPercentageVM>();
            LabSample currentSample = new LabSample();
            foreach (string sampleNb in distinctSamples)
            {
                List<LabSample> recordsOfOneSample = labSamples.Where(s => s.SampleNb == sampleNb).ToList();
                bool non_quantified = false;
                foreach (LabSample sample in recordsOfOneSample)
                {
                    currentSample = sample;
                    double conFinal_double = 0;
                    if (sample.ConFinal == "NQ")
                    {
                        non_quantified = true;
                    }
                    else if (sample.ConFinal != "NQ" && sample.ConFinal != "ND" && Double.TryParse(sample.ConFinal, out conFinal_double))
                    {
                        if (sample.LOQ != null && sample.LOD != null && conFinal_double > sample.LOD && conFinal_double < sample.LOQ)
                            non_quantified = true;

                    }
                }
                if (non_quantified)
                {
                    string chemicalName = db.ContaminantTypes.Where(c => c.pkContaminantTypeId == currentSample.fkContaminantTypeId).First().ContaminantName;
                    if (!result.ContainsKey(chemicalName))
                    {
                        NumberPercentageVM numberPercentageVM = new NumberPercentageVM();
                        numberPercentageVM.number = 1;
                        int totalCount = labSamples.Where(l => l.fkContaminantTypeId == currentSample.fkContaminantTypeId).ToList().Count;
                        if (totalCount > 0) numberPercentageVM.percentage = numberPercentageVM.number * 100 / totalCount;
                        result.Add(chemicalName, numberPercentageVM);
                    }
                    else //increment count in dictionary
                    {
                        foreach (var value in result.Keys)
                        {
                            if (value == chemicalName)
                            {
                                result[value].number++;
                                int totalCount = labSamples.Where(l => l.fkContaminantTypeId == currentSample.fkContaminantTypeId).ToList().Count;
                                if (totalCount > 0) result[value].percentage = result[value].number * 100 / totalCount;
                            }
                        }
                    }
                }
            }
            return result;
        }

        //5-Percentage and number of total violating samples(Conc final > ML/MRL) per contaminant/residue and per commodity
        public static List<CommConClassifcation> GetNbOfSamples_ViolatingForCommCon(List<LabSample> labSamples)
        {
            Dictionary<string, List<CommConClassifcation>> result = new Dictionary<string, List<CommConClassifcation>>();
            List<CommConClassifcation> commConClassifcations = new List<CommConClassifcation>();
            List<Commodity> distinctCommodities = GetDistinctCommoditiesForFindings(labSamples);
            foreach (Commodity commodity in distinctCommodities)
            {
                List<LabSample> samplesForSpecificCommodity = labSamples.Where(s => s.fkCommodityId == commodity.pkCommodityId).ToList();
                List<Mycotoxin> distinctMycotoxins = GetDistinctMycotoxinsForFindings(samplesForSpecificCommodity);
                List<PesticideResidue> distinctPesticideResidues = GetDistinctResiduesForFindings(samplesForSpecificCommodity);
                if (distinctMycotoxins != null && distinctMycotoxins.Count > 0)
                {
                    foreach (Mycotoxin mycotoxin in distinctMycotoxins)
                    {
                        CommConClassifcation commConClassifcation = new CommConClassifcation();
                        commConClassifcation.contaminant = mycotoxin.MycotoxinName;
                        commConClassifcation.commodity = commodity.CommodityName;
                        int count = labSamples.Where(s => s.fkCommodityId == commodity.pkCommodityId && s.fkMycotoxinId == mycotoxin.pkMycotoxinId && s.ComplianceResult == "V").ToList().Count();
                        commConClassifcation.nbOfSamples = count;
                        //int totalSamples = labSamples.Where(s => s.fkCommodityId == commodity.pkCommodityId && s.fkMycotoxinId == mycotoxin.pkMycotoxinId).ToList().Count();
                        int totalSamples = labSamples.Where(s => s.fkCommodityId == commodity.pkCommodityId).ToList().Count();
                        commConClassifcation.percentage = Math.Round(((double)count * 100 / totalSamples), 2);
                        //commConClassifcation.percentage = count * 100 / totalSamples;
                        commConClassifcations.Add(commConClassifcation);
                    }

                }
                if (distinctPesticideResidues != null && distinctPesticideResidues.Count > 0)
                {
                    foreach (PesticideResidue residue in distinctPesticideResidues)
                    {
                        CommConClassifcation commConClassifcation = new CommConClassifcation();
                        int count = labSamples.Where(s => s.fkCommodityId == commodity.pkCommodityId && s.fkPesticideResidueId == residue.pkPesticideResidueId && s.ComplianceResult == "V").ToList().Count();
                        commConClassifcation.contaminant = residue.PestResName;
                        commConClassifcation.commodity = commodity.CommodityName;
                        commConClassifcation.nbOfSamples = count;
                        //int totalSamples = labSamples.Where(s => s.fkCommodityId == commodity.pkCommodityId && s.fkPesticideResidueId == residue.pkPesticideResidueId).ToList().Count();
                        // int totalSamples = labSamples.Where(s => s.fkCommodityId == commodity.pkCommodityId).ToList().Count();
                        int totalSamples = DataAccess.getCountDistinctLabSamplesForCommodity(commodity.pkCommodityId);

                        commConClassifcation.percentage = Math.Round(((double)count * 100 / totalSamples), 2);
                        commConClassifcations.Add(commConClassifcation);
                    }

                }
            }
            return commConClassifcations;
        }

        //5(+)-Percentage and number of total detected unauthorized samples(Conc final > ML/MRL) per contaminant/residue and per commodity
        public static List<CommConClassifcation> GetNbOfSamples_UnauthorizedForCommCon(List<LabSample> labSamples)
        {
            Dictionary<string, List<CommConClassifcation>> result = new Dictionary<string, List<CommConClassifcation>>();
            List<CommConClassifcation> commConClassifcations = new List<CommConClassifcation>();
            List<Commodity> distinctCommodities = GetDistinctCommoditiesForFindings(labSamples);
            foreach (Commodity commodity in distinctCommodities)
            {
                List<LabSample> samplesForSpecificCommodity = labSamples.Where(s => s.fkCommodityId == commodity.pkCommodityId).ToList();
                List<PesticideResidue> distinctPesticideResidues = GetDistinctUnauthorizedResiduesForFindings(samplesForSpecificCommodity);
                if (distinctPesticideResidues != null && distinctPesticideResidues.Count > 0)
                {
                    foreach (PesticideResidue residue in distinctPesticideResidues)
                    {

                        if (residue != null)
                        {
                            CommConClassifcation commConClassifcation = new CommConClassifcation();
                            int count = labSamples.Where(s => s.fkCommodityId == commodity.pkCommodityId && s.fkPesticideResidueId == residue.pkPesticideResidueId).ToList().Count();
                            commConClassifcation.contaminant = residue.PestResName;
                            commConClassifcation.commodity = commodity.CommodityName;
                            commConClassifcation.nbOfSamples = count;
                            //int totalSamples = labSamples.Where(s => s.fkCommodityId == commodity.pkCommodityId && s.fkPesticideResidueId == residue.pkPesticideResidueId).ToList().Count();
                            int totalSamples = labSamples.Where(s => s.fkCommodityId == commodity.pkCommodityId).ToList().Count();
                            if (totalSamples > 0)
                                commConClassifcation.percentage = Math.Round(((double)count * 100 / totalSamples), 2);
                            //commConClassifcation.percentage = count * 100 / totalSamples;
                            else commConClassifcation.percentage = 0;
                            commConClassifcations.Add(commConClassifcation);
                        }
                    }
                }
            }
            return commConClassifcations;
        }


        public class CommConClassifcation
        {
            public string commodity { get; set; }
            public string contaminant { get; set; }
            public int nbOfSamples { get; set; }
            public double percentage { get; set; }
        }

        //Total number of detected residues in a commodity(only for PR)
        //here no LOD and LOQ so it is enough to check if it is double and > 0
        public static Dictionary<string,int> GetNbOfDetectedResiduesForCommodity(List<LabSample> labSamples)
        {
            Dictionary<string, int> CommodityResidues = new Dictionary<string, int>();
            List<Commodity> distinctCommodities = GetDistinctCommoditiesForFindings(labSamples);
            foreach(Commodity commodity in distinctCommodities)
            {
                //get list of labsamples for specified commodity where PR is detected
                List<LabSample> currentLabSamples = labSamples.Where(s => s.fkCommodityId == commodity.pkCommodityId).ToList();
                List<LabSample> LabSamples_detectedPR = currentLabSamples.Where(s => s.ConFinal != null && s.ConFinal.Trim() != "ND" && s.ConFinal.Trim() != "NQ" && Double.Parse(s.ConFinal.Trim()) > 0).ToList();
                int PRsCount = GetDistinctResiduesForFindings(LabSamples_detectedPR).Count;
                //int PRsCount = GetDistinctResiduesForFindings(labSamples.Where(s=>s.fkCommodityId == commodity.pkCommodityId
                //    && s.ConFinal.Trim() != "ND" && s.ConFinal.Trim() != "NQ" && s.ConFinal != null && Double.Parse(s.ConFinal) > 0).ToList()).Count;
                CommodityResidues.Add(commodity.CommodityCode + " - " + commodity.CommodityName, PRsCount);
            }
            return CommodityResidues;
        }

        //Number of detected non-authorized substances per commodity (only for PR)
        public static Dictionary<string, int> GetNbOfDetectedUnauthorizedForCommodity(List<LabSample> labSamples)
        {
            Dictionary<string, int> CommodityResidues = new Dictionary<string, int>();
            List<Commodity> distinctCommodities = GetDistinctCommoditiesForFindings(labSamples);
            foreach (Commodity commodity in distinctCommodities)
            {
                //get list of labsamples for specified commodity where PR is detected
                List<LabSample> currentLabSamples = labSamples.Where(s => s.fkCommodityId == commodity.pkCommodityId).ToList();
                List<PesticideResidue> distinctPesticideResidues = GetDistinctDetectedUnauthorizedResiduesForFindings(currentLabSamples);
               
                int PRsCount = distinctPesticideResidues.Count;
                CommodityResidues.Add(commodity.CommodityCode + " - " + commodity.CommodityName, PRsCount);
            }
            return CommodityResidues;
        }

        public static Dictionary<string, List<string>> GetListDetectedUnauthorizedForCommodity(List<LabSample> labSamples)
        {
            Dictionary<string, List<string>> CommodityResidues = new Dictionary<string, List<string>>();
            List<Commodity> distinctCommodities = GetDistinctCommoditiesForFindings(labSamples);
            foreach (Commodity commodity in distinctCommodities)
            {
                //get list of labsamples for specified commodity where PR is detected
                List<LabSample> currentLabSamples = labSamples.Where(s => s.fkCommodityId == commodity.pkCommodityId).ToList();
                List<PesticideResidue> distinctPesticideResidues = GetDistinctDetectedUnauthorizedResiduesForFindings(currentLabSamples);

                List<string> residuesNames = new List<string>();
                foreach(PesticideResidue p in distinctPesticideResidues)
                {
                    if(p != null)
                    residuesNames.Add(p.PestResName);
                }
                CommodityResidues.Add(commodity.CommodityCode + " - " + commodity.CommodityName, residuesNames);
            }
            return CommodityResidues;
        }

        //Maximum number “Max” of detected residue in one sample at a time per commodity(only for PR)
        public static Dictionary<string, int> GetMaxNbOfDetectedResidueForSpecificSampleNumber(List<LabSample> labSamples)
        {
            Dictionary<string, int> CommodityResidues = new Dictionary<string, int>();
            List<Commodity> distinctCommodities = GetDistinctCommoditiesForFindings(labSamples);
            foreach (Commodity commodity in distinctCommodities)
            {
                int PRsCount = 0;
                List<string> distinctSampleNumbers = (from a in labSamples
                                                      select a.SampleNb).Distinct().ToList();
                foreach (string sampleNb in distinctSampleNumbers)
                {
                    List<LabSample> recordsOfOneSample_detected = labSamples.Where(s => s.SampleNb == sampleNb && s.ConFinal != null && s.ConFinal.Trim() != "ND" && s.ConFinal.Trim() != "NQ" && Double.Parse(s.ConFinal.Trim()) > 0).ToList();
                    PRsCount = GetDistinctResiduesForFindings(recordsOfOneSample_detected).Count;
                    if(CommodityResidues.Count == 0)
                        CommodityResidues.Add(commodity.CommodityName, PRsCount);
                    else if (PRsCount > 0 && countIsNewMax(PRsCount, CommodityResidues.Values.ToList()))
                        CommodityResidues[commodity.CommodityName] = PRsCount;
                    //CommodityResidues.Add(commodity.CommodityName + " - Sample number: " + sampleNb, PRsCount);
                } 
            }
            //filter commodityResiduesMap to contain only the max 

            return CommodityResidues;
        } 
        private static bool countIsNewMax(int count, List<int> values)
        {
            bool newMax = false;
            if (values == null || (values != null && values.Count == 0))
                newMax = true;
            else
            {
                foreach (int i in values)
                {
                    if (count > i)
                        newMax = true;
                }
            }
            return newMax;
        }
        private static Dictionary<string, int> CountOfDetectedPRPerSample(List<LabSample> labSamples)
        {
            Dictionary<string, int> CountOfPRInSamples = new Dictionary<string, int>();
            List<string> distinctSampleNumbers = (from a in labSamples
                                                  select a.SampleNb).Distinct().ToList();
            foreach (string sampleNb in distinctSampleNumbers)
            {
                List<LabSample> recordsOfOneSample_detected = labSamples.Where(s => s.SampleNb == sampleNb
                            && s.ConFinal != null && s.ConFinal.Trim() != null && s.ConFinal.Trim() != "ND" && s.ConFinal.Trim() != "NQ" && Double.Parse(s.ConFinal) >0).ToList();
                int PRsCount = 0;
                PRsCount = GetDistinctResiduesForFindings(recordsOfOneSample_detected).Count;
                CountOfPRInSamples.Add(sampleNb, PRsCount);
            }
            return CountOfPRInSamples;
        }

        //Percentage and number of samples with one residue only per commodity(only for PR)
        public static int GetNbOfSamples_OneResidueOnlyPerCommodity(List<LabSample> labSamples)
        {
            Dictionary<string, int> CountOfPRInSamples = new Dictionary<string, int>();
            CountOfPRInSamples = CountOfDetectedPRPerSample(labSamples);
            int count = 0;
            //loop over values, if value = 1 add to list
            foreach (var key in CountOfPRInSamples.Keys)
            {
                if (CountOfPRInSamples[key] == 1)
                    count++;
            }
           
            return count;
        }

        //Percentage and number of samples with two residue per commodity(only for PR)
        public static int GetNbOfSamples_TwoResiduePerCommodity(List<LabSample> labSamples)
        {
            Dictionary<string, int> CountOfPRInSamples = new Dictionary<string, int>();
            CountOfPRInSamples = CountOfDetectedPRPerSample(labSamples);
            int count = 0;
            //loop over values, if value = 1 add to list
            foreach (var key in CountOfPRInSamples.Keys)
            {
                if (CountOfPRInSamples[key] == 2)
                    count++;
            }

            return count;
        }

        //Percentage and number of samples with three to 10 residues per commodity(only for PR)
        public static int GetNbOfSamples_ThreeToTenResiduePerCommodity(List<LabSample> labSamples)
        {
            Dictionary<string, int> CountOfPRInSamples = new Dictionary<string, int>();
            CountOfPRInSamples = CountOfDetectedPRPerSample(labSamples);
            int count = 0;
            //loop over values, if value = 1 add to list
            foreach (var key in CountOfPRInSamples.Keys)
            {
                if (CountOfPRInSamples[key] >=3 && CountOfPRInSamples[key] < 10)
                    count++;
            }

            return count;
        }

        //Percentage and number of samples with 10 to “Max” residues per commodity(only for PR)
        public static int GetNbOfSamples_TenToMaxResiduePerCommodity(List<LabSample> labSamples)
        {
            Dictionary<string, int> CountOfPRInSamples = new Dictionary<string, int>();
            CountOfPRInSamples = CountOfDetectedPRPerSample(labSamples);
            int count = 0;
            int Max = 0;
            foreach (var key in CountOfPRInSamples.Keys)
            {
                if (CountOfPRInSamples[key] > Max)
                    Max = CountOfPRInSamples[key];
            }
            foreach (var key in CountOfPRInSamples.Keys)
            {
                if (CountOfPRInSamples[key] >= 10 && CountOfPRInSamples[key] <= Max)
                    count++;
            }

            return count;
        }

        //Percentage and number of samples with residue detection for each pesticide molecule(per commodity) (only for PR)
        public static Dictionary<string, int> GetNbOfSamples_DetectionPerMolecule(List<LabSample> labSamples)
        {
            List<PesticideResidue> distinctPRs = GetDistinctResiduesForFindings(labSamples);
            Dictionary<string, int> CountOfSamplesForPR = new Dictionary<string, int>();
            foreach (PesticideResidue pr in distinctPRs)
            {
                int count = labSamples.Where(s => s.fkPesticideResidueId == pr.pkPesticideResidueId).ToList().Count;
                CountOfSamplesForPR.Add(pr.PestResName, count);
            }

            return CountOfSamplesForPR;

        }

        //Percentage and number of violating samples per pesticide molecule per commodity(Conc final > MRL) (only for PR)
        public static Dictionary<string, NumberPercentageVM> GetNbOfSamples_ViolatingPerMoleculePerCommodity(List<LabSample> labSamples)
        {
            List<PesticideResidue> distinctPRs = GetDistinctResiduesForFindings(labSamples);
            Dictionary<string, NumberPercentageVM> CountOfViolatingSamplesForPR = new Dictionary<string, NumberPercentageVM>();
            foreach (PesticideResidue pr in distinctPRs)
            {
                List<LabSample> labSamplesForPR = new List<LabSample>();
                labSamplesForPR = labSamples.Where(s => s.fkPesticideResidueId == pr.pkPesticideResidueId).ToList();
                int total = labSamplesForPR.Count;
                int countViolating = labSamplesForPR.Where(s => s.ComplianceResult == "V" && !s.ViolationType.Contains("banned")).ToList().Count;
                NumberPercentageVM numberPercentageVM = new NumberPercentageVM();
                if (countViolating > 0)
                {
                    numberPercentageVM.number = countViolating;
                    if(total>0 && countViolating>0)
                        numberPercentageVM.percentage = Math.Round(((double)countViolating * 100 / total), 2);
                    //numberPercentageVM.percentage = countViolating * 100 / total;
                    CountOfViolatingSamplesForPR.Add(pr.PestResName, numberPercentageVM);
                }
            }
            return CountOfViolatingSamplesForPR;
        }


        private static Dictionary<string, int> MoleculesInSamplesCount(List<LabSample> labSamplesForSpecificCommodity)
        {
            Dictionary<string, int> NbOfSamplesForEachPR = new Dictionary<string, int>();
            List<PesticideResidue> distinctMolecules = GetDistinctResiduesForFindings(labSamplesForSpecificCommodity);
            foreach (PesticideResidue pr in distinctMolecules)
            {
                double i = 0;
                int count = labSamplesForSpecificCommodity.Where(s => s.fkPesticideResidueId == pr.pkPesticideResidueId 
                    && s.ConFinal !=null && s.ConFinal.Trim() != "NQ" && s.ConFinal.Trim() != "ND" && Double.TryParse(s.ConFinal,out i))
                    .ToList().Count;
                NbOfSamplesForEachPR.Add(pr.PestResName, count);
            }
            return NbOfSamplesForEachPR;
        }

        //Percentage and number of pesticide molecules detected in one sample only per commodity(only for PR)
        public static int GetNbOfMolecules_DetectedInOneSampleOnlyPerComm(List<LabSample> labSamplesForSpecificCommodity)
        {
            Dictionary<string, int> NbOfSamplesForEachPR = new Dictionary<string, int>();
            NbOfSamplesForEachPR = MoleculesInSamplesCount(labSamplesForSpecificCommodity);
            int countOfPRInOneSample = 0;
            foreach (var key in NbOfSamplesForEachPR.Keys)
            {
                if (NbOfSamplesForEachPR[key] == 1)
                {
                    countOfPRInOneSample++;
                }
            }

            return countOfPRInOneSample;
        }

        //Percentage and number of pesticide molecules detected in two to ten samples per commodity(only for PR)
        public static int GetNbOfMolecules_DetectedIn2To10SamplesPerComm(List<LabSample> labSamples)
        {
            Dictionary<string, int> NbOfSamplesForEachPR = new Dictionary<string, int>();
            NbOfSamplesForEachPR = MoleculesInSamplesCount(labSamples);
            int countOfPR = 0;
            foreach (var key in NbOfSamplesForEachPR.Keys)
            {
                if (NbOfSamplesForEachPR[key] >= 2 && NbOfSamplesForEachPR[key] <= 10)
                {
                    countOfPR++;
                }
            }

            return countOfPR;
        }

        //Percentage and number of pesticide molecules detected in eleven to 50 samples per commodity(only for PR)
        public static int GetNbOfMolecules_DetectedIn11To50SamplesPerComm(List<LabSample> labSamples)
        {
            Dictionary<string, int> NbOfSamplesForEachPR = new Dictionary<string, int>();
            NbOfSamplesForEachPR = MoleculesInSamplesCount(labSamples);
            int countOfPR = 0;
            foreach (var key in NbOfSamplesForEachPR.Keys)
            {
                if (NbOfSamplesForEachPR[key] >= 11 && NbOfSamplesForEachPR[key] <= 50)
                {
                    countOfPR++;
                }
            }

            return countOfPR;
        }

        //Percentage and number of pesticide molecules detected in 51 to 100 samples per commodity(only for PR)
        public static int GetNbOfMolecules_DetectedIn51To100SamplesPerComm(List<LabSample> labSamples)
        {
            Dictionary<string, int> NbOfSamplesForEachPR = new Dictionary<string, int>();
            NbOfSamplesForEachPR = MoleculesInSamplesCount(labSamples);
            int countOfPR = 0;
            foreach (var key in NbOfSamplesForEachPR.Keys)
            {
                if (NbOfSamplesForEachPR[key] >= 51 && NbOfSamplesForEachPR[key] <= 100)
                {
                    countOfPR++;
                }
            }

            return countOfPR;
        }

        //Percentage and number of pesticide molecules detected in 101 to 1000 samples per commodity(only for PR)
        public static int GetNbOfMolecules_DetectedIn101To1000SamplesPerComm(List<LabSample> labSamples)
        {
            Dictionary<string, int> NbOfSamplesForEachPR = new Dictionary<string, int>();
            NbOfSamplesForEachPR = MoleculesInSamplesCount(labSamples);
            int countOfPR = 0;
            foreach (var key in NbOfSamplesForEachPR.Keys)
            {
                if (NbOfSamplesForEachPR[key] >= 101 && NbOfSamplesForEachPR[key] <= 1000)
                {
                    countOfPR++;
                }
            }

            return countOfPR;
        }

        //Percentage and number of pesticide molecules detected in more than 1000 samples per commodity(only for PR)
        public static int GetNbOfMolecules_DetectedInMoreThan1000SamplesPerComm(List<LabSample> labSamples)
        {
            Dictionary<string, int> NbOfSamplesForEachPR = new Dictionary<string, int>();
            NbOfSamplesForEachPR = MoleculesInSamplesCount(labSamples);
            int countOfPR = 0;
            foreach (var key in NbOfSamplesForEachPR.Keys)
            {
                if (NbOfSamplesForEachPR[key] > 1000)
                {
                    countOfPR++;
                }
            }

            return countOfPR;
        }

        //Number of pesticide molecules with at least one sample per commodity exceeding MRL(Conc final > MRL) (only for PR)
        public static int GetNbOfMolecules_AtLeastOneViolatingSamplePerComm(List<LabSample> labSamplesForSpecificCommodity)
        {
            List<PesticideResidue> distinctMolecules = GetDistinctResiduesForFindings(labSamplesForSpecificCommodity);
            List<PesticideResidue> _atLeastOneViolatingSample = new List<PesticideResidue>();
            foreach (PesticideResidue pr in distinctMolecules)
            {
                var listOfViolating =  labSamplesForSpecificCommodity.Where(s => s.fkPesticideResidueId == pr.pkPesticideResidueId 
                            && s.ComplianceResult == "V" && s.ViolationType.Contains("Exceeding"));
                if (listOfViolating.ToList().Count > 0)
                    _atLeastOneViolatingSample.Add(pr);
            }

            return _atLeastOneViolatingSample.Count;
        }


        //Get Map of DetailedFindings according to governorates
        //public static Dictionary<string, DetailedFindings> GetMapOfDetailedFindings_Gov(List<LabSample> labSamples)
        //{
        //    Dictionary<string, DetailedFindings> result = new Dictionary<string, DetailedFindings>();
        //    List<Governorate> governorates = GetDistinctGovernoratesForSamples(labSamples);
        //    foreach(Governorate gov in governorates)
        //    {
        //        List<LabSample> labSample_gov = labSamples.Where(s => s.fkGovernorateId == gov.pkGovernorateId).ToList();
        //        string keyName = "( " + gov.GovernorateCode + " ) " + gov.GovernorateName;
        //        DetailedFindings detailedFindings = new DetailedFindings
        //        {
        //            AtLeastOneDetectedMoleculeInSamples = StatisticalAccess.GetNbOfSamples_AtLeastOneDetectedMoleculeForCommCon(labSample_gov),
        //            noDetectedMoleculeInSamples = StatisticalAccess.GetNbOfSamples_NoDetectedMoleculeForCommCon(labSample_gov),
        //            quantifiedSamplesCount = StatisticalAccess.GetNbOfSamples_QuantifiedDetectionForCommCon(labSample_gov),
        //            notQuantifiedSamplesCount = StatisticalAccess.GetNbOfSamples_NoQuantifiedDetectionForCommCon(labSample_gov),
        //            commConClassifcations = StatisticalAccess.GetNbOfSamples_ViolatingForCommCon(labSample_gov),
        //            detectedPRForSpecificCommodity = StatisticalAccess.GetNbOfDetectedResiduesForCommodity(labSample_gov),
        //            maxDetectedPRInSample = StatisticalAccess.GetMaxNbOfDetectedResidueForSpecificSampleNumber(labSample_gov),
        //            samplesWithOnePRCount = StatisticalAccess.GetNbOfSamples_OneResidueOnlyPerCommodity(labSample_gov),
        //            samplesWithTwoPRCount = StatisticalAccess.GetNbOfSamples_TwoResiduePerCommodity(labSample_gov),
        //            samplesWith3to10PRCount = StatisticalAccess.GetNbOfSamples_ThreeToTenResiduePerCommodity(labSample_gov),
        //            samplesWith10toMaxPRCount = StatisticalAccess.GetNbOfSamples_TenToMaxResiduePerCommodity(labSample_gov),
        //            countSamplesOfSpecificPR = StatisticalAccess.GetNbOfSamples_DetectionPerMolecule(labSample_gov),
        //            countViolatingSamplesPerPR = StatisticalAccess.GetNbOfSamples_ViolatingPerMoleculePerCommodity(labSample_gov),
        //            countPRDetectedIn1sample = StatisticalAccess.GetNbOfMolecules_DetectedInOneSampleOnlyPerComm(labSample_gov),
        //            countPRDetectedIn2to10sample = StatisticalAccess.GetNbOfMolecules_DetectedIn2To10SamplesPerComm(labSample_gov),
        //            countPRDetectedIn11to50sample = StatisticalAccess.GetNbOfMolecules_DetectedIn11To50SamplesPerComm(labSample_gov),
        //            countPRDetectedIn51to100sample = StatisticalAccess.GetNbOfMolecules_DetectedIn51To100SamplesPerComm(labSample_gov),
        //            countPRDetectedIn101to1000sample = StatisticalAccess.GetNbOfMolecules_DetectedIn101To1000SamplesPerComm(labSample_gov),
        //            countPRDetectedInMorethan1000sample = StatisticalAccess.GetNbOfMolecules_DetectedInMoreThan1000SamplesPerComm(labSample_gov),
        //            countPRsWithOneViolating = StatisticalAccess.GetNbOfMolecules_AtLeastOneViolatingSamplePerComm(labSample_gov)
        //        };
        //        result.Add(keyName, detailedFindings);
        //    }
        //    return result;
        //}

        public static Dictionary<string, List<FindingsStat>> GetMapOfFindingsStat_Gov(List<LabSample> labSamples)
        {
            Dictionary<string, List<FindingsStat>> result = new Dictionary<string, List<FindingsStat>>();
            List<Governorate> governorates = GetDistinctGovernoratesForSamples(labSamples);
            foreach (Governorate gov in governorates)
            {
                string keyName = "( " + gov.GovernorateCode + " ) " + gov.GovernorateName;
                List<LabSample> labSample_gov = labSamples.Where(s => s.fkGovernorateId == gov.pkGovernorateId).ToList();
                List<FindingsStat> findingsStats_gov = GetAllFindingStatRecords(labSample_gov);
                result.Add(keyName, findingsStats_gov);

            }
            return result;
        }

        public static Dictionary<string, List<FindingsStat>> GetMapOfFindingsStat_Origin(List<LabSample> labSamples, List<SampleOrigin> origins)
        {
            Dictionary<string, List<FindingsStat>> result = new Dictionary<string, List<FindingsStat>>();
            foreach (SampleOrigin org in origins)
            {
                string keyName = "( " + org.OriginCode + " ) " + org.OriginText;
                List<LabSample> labSample_org = labSamples.Where(s => s.fkSampleOriginId == org.pkSampleOriginId).ToList();
                List<FindingsStat> findingsStats_org = GetAllFindingStatRecords(labSample_org);
                result.Add(keyName, findingsStats_org);

            }
            return result;
        }

        public static Dictionary<string, List<FindingsStat>> GetMapOfFindingsStat_Premise(List<LabSample> labSamples, List<string> premiseTypes)
        {
            Dictionary<string, List<FindingsStat>> result = new Dictionary<string, List<FindingsStat>>();
            foreach (string pre in premiseTypes)
            {
                string keyName = pre;
                List<LabSample> labSample_pre = labSamples.Where(s => s.PremiseType == pre).ToList();
                List<FindingsStat> findingsStats_pre = GetAllFindingStatRecords(labSample_pre);
                result.Add(keyName, findingsStats_pre);

            }
            return result;
        }

        public static Dictionary<string, double> GetMapOfLeftProportions_Gov(List<LabSample> labSamples)
        {
            Dictionary<string, double> result = new Dictionary<string, double>();
            List<Governorate> governorates = GetDistinctGovernoratesForSamples(labSamples);
            foreach (Governorate gov in governorates)
            {
                string keyName = "( " + gov.GovernorateCode + " ) " + gov.GovernorateName;
                List<LabSample> labSample_gov = labSamples.Where(s => s.fkGovernorateId == gov.pkGovernorateId).ToList();
                double leftProportion = GetProportionOfLeftCensoredData(labSample_gov);
                result.Add(keyName, leftProportion);
            }
            return result;
        }

        public static Dictionary<string, double> GetMapOfLeftProportions_Premise(List<LabSample> labSamples, List<string> premiseTypes)
        {
            Dictionary<string, double> result = new Dictionary<string, double>();
            foreach (string pre in premiseTypes)
            {
                string keyName = pre;
                List<LabSample> labSample_pre = labSamples.Where(s => s.PremiseType == pre).ToList();
                double leftProportion = GetProportionOfLeftCensoredData(labSample_pre);
                result.Add(keyName, leftProportion);
            }
            return result;
        }

        public static Dictionary<string, double> GetMapOfLeftProportions_Origin(List<LabSample> labSamples, List<SampleOrigin> origins)
        {
            Dictionary<string, double> result = new Dictionary<string, double>();
            foreach (SampleOrigin org in origins)
            {
                string keyName = "( " + org.OriginCode + " ) " + org.OriginText;
                List<LabSample> labSample_org = labSamples.Where(s => s.fkSampleOriginId == org.pkSampleOriginId).ToList();
                double leftProportion = GetProportionOfLeftCensoredData(labSample_org);
                result.Add(keyName, leftProportion);
            }
            return result;
        }

        public static Dictionary<string, int> GetTotalOfSamples_Gov(List<LabSample> labSamples)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            List<Governorate> governorates = GetDistinctGovernoratesForSamples(labSamples);
            foreach (Governorate gov in governorates)
            {
                string keyName = "( " + gov.GovernorateCode + " ) " + gov.GovernorateName;
                List<LabSample> labSample_gov = labSamples.Where(s => s.fkGovernorateId == gov.pkGovernorateId).ToList();
                result.Add(keyName, labSample_gov.Count);
            }
            return result;
        }

        public static Dictionary<string, int> GetTotalOfSamples_Origin(List<LabSample> labSamples, List<SampleOrigin> origins)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            List<SampleOrigin> sampleOrigins = DataAccess.GetDistinctOriginsForSamples(labSamples);
            foreach (SampleOrigin origin in sampleOrigins)
            {
                string keyName = "( " + origin.OriginCode + " ) " + origin.OriginText;
                List<LabSample> labSample_org = labSamples.Where(s => s.fkSampleOriginId == origin.pkSampleOriginId).ToList();
                result.Add(keyName, labSample_org.Count);
            }
            return result;
        }

        public static Dictionary<string, int> GetTotalOfSamples_PremiseType(List<LabSample> labSamples, List<string> premiseTypes)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            foreach (string premiseType in premiseTypes)
            {
                string keyName = premiseType;
                List<LabSample> labSample_pre = labSamples.Where(s => s.PremiseType.Trim() == premiseType.Trim()).ToList();
                result.Add(keyName, labSample_pre.Count);
            }
            return result;
        }

        public static int GetTotalOfSamples_All(List<LabSample> labSamples)
        {
            List<string> distinctLabSamples = (from a in labSamples where !String.IsNullOrEmpty(a.SampleNb)
                           select a.SampleNb).Distinct().ToList();
            return distinctLabSamples.Count;
        }

        public static int GetTotalOfResultSamples_All(List<LabSample> labSamples)
        {
            return labSamples.Count;
        }

    }
}