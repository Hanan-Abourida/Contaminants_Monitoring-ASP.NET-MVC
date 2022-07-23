using Contaminants_Monitoring.Models;
using Contaminants_Monitoring.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Contaminants_Monitoring.Controllers
{
    public class LabTestController : Controller
    {

        private List<LabSample> initInventory()
        {
            List<LabSample> labSamples = new List<LabSample>();
            string userId = User.Identity.GetUserId();
            labSamples = UsersDataAccess.GetAllSamplesDispatchedForOneLab(userId);
            return labSamples;
        }

        // GET: LabTest
        public ActionResult Inventory()
        {
            LabInventoryViewModel labInventoryViewModel = new LabInventoryViewModel();
            labInventoryViewModel.labSamples = initInventory();
            return View(labInventoryViewModel);
        }

        [HttpPost]
        public ActionResult Inventory(LabInventoryViewModel labInventoryViewModel)
        {
            string userId = User.Identity.GetUserId();
            if (!String.IsNullOrEmpty(labInventoryViewModel.status) && labInventoryViewModel.status == "Dispatched")
                labInventoryViewModel.labSamples = initInventory();
            else if(!String.IsNullOrEmpty(labInventoryViewModel.status) && labInventoryViewModel.status == "SentForTesting")
                labInventoryViewModel.labSamples = UsersDataAccess.initInventoryWithSent(userId);
            else if (!String.IsNullOrEmpty(labInventoryViewModel.status) && labInventoryViewModel.status == "Rejected")
                labInventoryViewModel.labSamples = UsersDataAccess.initInventoryWithRejected(userId);
            else if (!String.IsNullOrEmpty(labInventoryViewModel.status) && labInventoryViewModel.status == "All")
                labInventoryViewModel.labSamples = UsersDataAccess.initInventoryWithAll(userId);

            Session["LabInventory"] = labInventoryViewModel.labSamples;
            return View(labInventoryViewModel);
        }

        [HttpGet]
        public ActionResult EditSampleByLab(int? id)
        {
            LabViewModel labViewModel = new LabViewModel();
            LabSample labSample = DataAccess.GetLabSampleFromId(id);
            labViewModel.labSample = labSample;
            labViewModel = readSampleState(labViewModel);
            labViewModel.user = UsersDataAccess.GetCollectorFromSampleId((int)id);
            return View(labViewModel);
        }

        //[HttpGet]
        //public ActionResult Details(int? id)
        //{
        //    LabViewModel labViewModel = new LabViewModel();
        //    LabSample labSample = DataAccess.GetLabSampleFromId(id);
        //    labViewModel.labSample = labSample;
        //    labViewModel = readSampleState(labViewModel);
        //    return View(labViewModel);
        //}

        [HttpPost]
        public ActionResult SaveLabData(LabViewModel labViewModel)
        {
            if(labViewModel.received && labViewModel.labSample.ReceivingDate == null)
            {
                ViewBag.ErrorMessage = "Must Enter the receiving date!";
                LabSample modellabSample = DataAccess.GetLabSampleFromId(labViewModel.labSample.pkLabSampleId);
                labViewModel.labSample = modellabSample;
                labViewModel = readSampleState(labViewModel);
                labViewModel.user = UsersDataAccess.GetCollectorFromSampleId(labViewModel.labSample.pkLabSampleId);
                return View("EditSampleByLab", labViewModel);
            }
            if (labViewModel.sentToTesting)
                labViewModel.labSample.Status = "Sent To Testing";
            else if(labViewModel.acceptedText != null && labViewModel.acceptedText.Equals("True"))
                labViewModel.labSample.Status = "Accepted For Testing";
            else if (labViewModel.acceptedText != null && labViewModel.acceptedText.Equals("False"))
                labViewModel.labSample.Status = "Rejected";
            else if(labViewModel.received)
                labViewModel.labSample.Status = "Received";
            bool updated = DataAccess.UpdateSampleFromLabInfo(labViewModel.labSample.pkLabSampleId, labViewModel.labSample);
            if (updated)
                ViewBag.Message = "Updated Successfully.";
            else ViewBag.ErrorMessage = "Error in saving updates!";

            LabSample labSample = DataAccess.GetLabSampleFromId(labViewModel.labSample.pkLabSampleId);
            labViewModel.labSample = labSample;
            labViewModel = readSampleState(labViewModel);
            labViewModel.user = UsersDataAccess.GetCollectorFromSampleId(labViewModel.labSample.pkLabSampleId);
            return View("EditSampleByLab",labViewModel);
        }

        private LabViewModel readSampleState(LabViewModel labViewModel)
        {
            if (!String.IsNullOrEmpty(labViewModel.labSample.Status))
            {
                if (labViewModel.labSample.Status == "Received")
                {
                    labViewModel.received = true;
                }
                else if (labViewModel.labSample.Status == "Accepted For Testing")
                {
                    labViewModel.received = true;
                    labViewModel.acceptedText = "True";
                }
                else if (labViewModel.labSample.Status == "Sent To Testing")
                {
                    labViewModel.received = true;
                    labViewModel.acceptedText = "True";
                    labViewModel.sentToTesting = true;
                }
                else if (labViewModel.labSample.Status == "Rejected")
                {
                    labViewModel.received = true;
                    labViewModel.acceptedText = "False";
                    labViewModel.sentToTesting = false;
                }
                else
                {
                    labViewModel.received = false;
                    labViewModel.acceptedText = "";
                    labViewModel.sentToTesting = false;
                }
            }
            return labViewModel;
        }

        [HttpGet]
        public ActionResult AddTest(int? id)
        {
            LabSample dbLabSample = DataAccess.GetLabSampleFromId(id);
            LabTestViewModel labTestViewModel = new LabTestViewModel();
            LabSample labSample = new LabSample();
            
            if (dbLabSample.fkMycotoxinId == null && dbLabSample.fkPesticideResidueId == null)
            {
                labSample = dbLabSample;
                labTestViewModel.labSample = labSample;
                //Here we are not supposed to edit, so all testing fields must be empty
                //labTestViewModel.labSample.LOD = null;
                //labTestViewModel.labSample.LOQ = null;
                //labTestViewModel.labSample.AnalysisDate = null;
                //labTestViewModel.labSample.AnalysisPortion = null;
                //labTestViewModel.labSample.AnalysisType = null;
                //labTestViewModel.labSample.ConFinal = null;
                //labTestViewModel.labSample.fkConUOM = null;
                //labTestViewModel.labSample.Uncertainty = null;
                //labTestViewModel.labSample.UncertaintyPercent = null;
                //labTestViewModel.labSample.ExtractionMethod = null;
                //labTestViewModel.labSample.DeterminativeMethod = null;
                //labTestViewModel.labSample.ViolationType = null;
                //labTestViewModel.labSample.CloseCaseDate = null;
                labTestViewModel.labSample.Status = "Sent To Testing";
            }
            else
            {
                //set values
                labSample = MergeSampleInfoData(labSample, dbLabSample);
                //set relations
                labSample.ContaminantType = dbLabSample.ContaminantType;
                labSample.Commodity = dbLabSample.Commodity;
                labSample.CommodityState = dbLabSample.CommodityState;
                labSample.Governorate = dbLabSample.Governorate;
                labSample.Caza = dbLabSample.Caza;
                labSample.SamplingReason = dbLabSample.SamplingReason;
                labSample.SamplingType = dbLabSample.SamplingType;
                labSample.SampleOrigin = dbLabSample.SampleOrigin;
                labSample.Country = dbLabSample.Country;
                labSample.SamplingType = dbLabSample.SamplingType;
                labSample.Laboratory = dbLabSample.Laboratory;
                labTestViewModel.labSample = labSample;
                if (dbLabSample.CloseCaseDate != null)
                    labTestViewModel.caseClosed = true;
            }

            labTestViewModel.pesticideResidues = DataAccess.getAllPesticideResidues();
            labTestViewModel.mycotoxins = DataAccess.getAllMycotoxins();
            labTestViewModel.units = DataAccess.GetAllUOMs();
            return View(labTestViewModel);
        }

       

        [HttpPost]
        public ActionResult AddTest(LabTestViewModel labTestViewModel)
        {
            
            if (labTestViewModel.labSample.AnalysisDate == null)
            {
                ViewBag.ErrorMessage = "Must Enter the Analysis Date!";
                return View("AddTest", resetView(labTestViewModel));
            }
            if (labTestViewModel.concentrationStatus == "D" && labTestViewModel.labSample.ConFinal == null)
            {
                ViewBag.ErrorMessage = "Must Enter the final concentration!";
                return View("AddTest", resetView(labTestViewModel));
            }
            if (labTestViewModel.concentrationStatus == "D" && labTestViewModel.selectedUOM == null)
            {
                ViewBag.ErrorMessage = "Must choose the final concentration unit!";
                return View("AddTest", resetView(labTestViewModel));
            }
            LabSample labSample = new LabSample();
            if (labTestViewModel.labSample.pkLabSampleId != 0)
                labSample = DataAccess.GetLabSampleFromId(labTestViewModel.labSample.pkLabSampleId);
            else
                labSample = DataAccess.GetFirstLabSampleFromSampleNb(labTestViewModel.labSample.SampleNb);
            //Get Values of test
            bool addResult = false;
            if (labSample.fkMycotoxinId == null && labSample.fkPesticideResidueId == null)
            {
                //updateLabSample
                labSample.AnalysisDate = labTestViewModel.labSample.AnalysisDate;

                if(labSample.ContaminantType.ContaminantCode == "R")
                    labSample.fkPesticideResidueId = labTestViewModel.selectedResID;
                else if (labSample.ContaminantType.ContaminantCode == "AF")
                    labSample.fkMycotoxinId = labTestViewModel.selectedMycotoxinId;

                if (labTestViewModel.caseClosed)
                    labSample.CloseCaseDate = DateTime.Now.Date;
                labSample.AnalysisPortion = labTestViewModel.labSample.AnalysisPortion;
                labSample.AnalysisType = labTestViewModel.labSample.AnalysisType;
                labSample.ExtractionMethod = labTestViewModel.labSample.ExtractionMethod;
                labSample.DeterminativeMethod = labTestViewModel.labSample.DeterminativeMethod;
                labSample.ConFinal = labTestViewModel.labSample.ConFinal;
                labSample.fkConUOM = labTestViewModel.selectedUOM;
                labSample.Uncertainty = labTestViewModel.labSample.Uncertainty;
                labSample.UncertaintyPercent = labTestViewModel.labSample.UncertaintyPercent;
                labSample.Status = labTestViewModel.labSample.Status;
                labSample.Lab_Rejection_Comment = labTestViewModel.labSample.Lab_Rejection_Comment;
                labSample.ViolationType = SetViolationType(labSample);
                addResult = DataAccess.UpdateLabTestInfo(labSample);
            }
            else
            {
                //new lab sample
                LabSample newLabSample = new LabSample();
                //Populate All Fields
                newLabSample = MergeSampleInfoData(newLabSample, labSample);

                newLabSample.AnalysisDate = labTestViewModel.labSample.AnalysisDate;

                ContaminantType newSampleType = DataAccess.GetContaminantTypeFromId((int)newLabSample.fkContaminantTypeId);
                if (newSampleType.ContaminantCode == "R")
                    newLabSample.fkPesticideResidueId = labTestViewModel.selectedResID;
                else if (newSampleType.ContaminantCode == "AF")
                    newLabSample.fkMycotoxinId = labTestViewModel.selectedMycotoxinId;

                newLabSample.AnalysisPortion = labTestViewModel.labSample.AnalysisPortion;
                newLabSample.AnalysisType = labTestViewModel.labSample.AnalysisType;
                newLabSample.ExtractionMethod = labTestViewModel.labSample.ExtractionMethod;
                newLabSample.DeterminativeMethod = labTestViewModel.labSample.DeterminativeMethod;
                if (labTestViewModel.concentrationStatus == "ND")
                {
                    newLabSample.ConFinal = "ND";
                }
                else if (labTestViewModel.concentrationStatus == "NQ")
                {
                    newLabSample.ConFinal = "NQ";
                }
                else newLabSample.ConFinal = labTestViewModel.labSample.ConFinal;
                newLabSample.fkConUOM = labTestViewModel.selectedUOM;
                newLabSample.Uncertainty = labTestViewModel.labSample.Uncertainty;
                newLabSample.UncertaintyPercent = labTestViewModel.labSample.UncertaintyPercent;
                newLabSample.Status = "Sent To Testing";
                newLabSample.Lab_Rejection_Comment = labTestViewModel.labSample.Lab_Rejection_Comment;
                if (labTestViewModel.caseClosed) newLabSample.CloseCaseDate = DateTime.Now.Date;
                newLabSample.ViolationType = SetViolationType(newLabSample);
                addResult = DataAccess.AddNewSingleLabSample(newLabSample);
                labTestViewModel.labSample = newLabSample;
            }
            if (addResult)
            {
                ViewBag.Message = "New lab test added successfully.";
                ViewBag.ErrorMessage = "";
            }
            else
            {
                ViewBag.ErrorMessage = "Error adding new test.";
                ViewBag.Message = "";
            }
            
            return View(resetView(labTestViewModel));
        }

        private LabTestViewModel resetView(LabTestViewModel labTestViewModelParam)
        {
          //  int id = labTestViewModelParam.labSample.pkLabSampleId;
            LabSample labSample = DataAccess.GetLabSampleFromId(labTestViewModelParam.labSample.pkLabSampleId);
            LabTestViewModel labTestViewModel = new LabTestViewModel();
            labTestViewModel.pesticideResidues = DataAccess.getAllPesticideResidues();
            labTestViewModel.mycotoxins = DataAccess.getAllMycotoxins();
            labTestViewModel.units = DataAccess.GetAllUOMs();
            labTestViewModel.labSample = labSample;
            if (labSample.ConFinal == "ND")
                labTestViewModel.concentrationStatus = "ND";
            else if(labSample.ConFinal == "NQ")
                labTestViewModel.concentrationStatus = "NQ";
            else if(!String.IsNullOrEmpty(labTestViewModel.labSample.ConFinal))
                labTestViewModel.concentrationStatus = "D";
            if (labSample.CloseCaseDate != null)
                labTestViewModel.caseClosed = true;
            return labTestViewModel;
        }

        [HttpGet]
        public ActionResult TestsList(int? id)
        {
            LabSample labSample = DataAccess.GetLabSampleFromId(id);
            List<LabTestViewModel> labTestViews = new List<LabTestViewModel>();
            if (labSample != null && !String.IsNullOrEmpty(labSample.SampleNb))
            {

                List<LabSample> samplesOfOneIdentity = DataAccess.getSamplesOfOneIdentity(labSample.SampleNb);
                foreach(LabSample item in samplesOfOneIdentity)
                {
                    LabTestViewModel labTestViewModel = new LabTestViewModel();
                    labTestViewModel.labSample = item;
                    labTestViewModel.caseClosed = item.Status == "CaseClosed" ? true : false;
                    labTestViews.Add(labTestViewModel);
                }
            }
            return View(labTestViews);
        }

        //Nothig to post here
        //unless filter is used
        [HttpPost]
        public ActionResult TestsList(OneSampleTestsViewModel oneSampleTestsViewModel)
        {
            return View();
        }

        private static LabSample MergeSampleInfoData(LabSample newLabSample ,LabSample labSampleFromModel)
        {
            newLabSample.SampleNb = labSampleFromModel.SampleNb;
            newLabSample.LabSampleNb = labSampleFromModel.LabSampleNb;
            newLabSample.SampleCode = labSampleFromModel.SampleNb;
            
            newLabSample.fkCommodityId = labSampleFromModel.fkCommodityId;
            newLabSample.fkContaminantTypeId = labSampleFromModel.fkContaminantTypeId;
            newLabSample.fkCommodityStateId = labSampleFromModel.fkCommodityStateId;
            newLabSample.fkGovernorateId = labSampleFromModel.fkGovernorateId;
            newLabSample.fkCazaId = labSampleFromModel.fkCazaId;
            newLabSample.fkSampleOriginId = labSampleFromModel.fkSampleOriginId;
            newLabSample.fkSamplingReasonId = labSampleFromModel.fkSamplingReasonId;
            newLabSample.fkSamplingTypeId = labSampleFromModel.fkSamplingTypeId;
            newLabSample.fkLaboratoryId = labSampleFromModel.fkLaboratoryId;
            newLabSample.fkSourceTypeId = labSampleFromModel.fkSourceTypeId;

            newLabSample.OfficerName = labSampleFromModel.OfficerName;
            newLabSample.OfficerPhoneNb = labSampleFromModel.OfficerPhoneNb;
            newLabSample.SamplingPlan = labSampleFromModel.SamplingPlan;
            newLabSample.PremiseName = labSampleFromModel.PremiseName;
            newLabSample.PremiseType = labSampleFromModel.PremiseType;
            newLabSample.PremiseAddress = labSampleFromModel.PremiseAddress;
            newLabSample.ContactName = labSampleFromModel.ContactName;
            newLabSample.ContactPhoneNumber = labSampleFromModel.ContactPhoneNumber;
            newLabSample.SourceName = labSampleFromModel.SourceName;
            newLabSample.SourceAddress = labSampleFromModel.SourceAddress;
            newLabSample.SourceContactName = labSampleFromModel.SourceContactName;
            newLabSample.SourceContactNumber = labSampleFromModel.SourceContactNumber;
            newLabSample.SamplingDate = labSampleFromModel.SamplingDate;
            newLabSample.SamplingTime = labSampleFromModel.SamplingTime;
            newLabSample.BrandName = labSampleFromModel.BrandName;
            newLabSample.Manufacturer = labSampleFromModel.Manufacturer;
            newLabSample.Distributor = labSampleFromModel.Distributor;
            newLabSample.Importer = labSampleFromModel.Importer;
            newLabSample.PackQuantitySize = labSampleFromModel.PackQuantitySize;
            newLabSample.BatchLotNumber = labSampleFromModel.BatchLotNumber;
            newLabSample.ShelfLife = labSampleFromModel.ShelfLife;
            newLabSample.StorageConditions = labSampleFromModel.StorageConditions;

            newLabSample.ReceivingDate = labSampleFromModel.ReceivingDate;
            newLabSample.ReceivingTime = labSampleFromModel.ReceivingTime;
            newLabSample.DispatchDate = labSampleFromModel.DispatchDate;
            newLabSample.Notes_Designer = labSampleFromModel.Notes_Designer;
            newLabSample.Notes_Collector = labSampleFromModel.Notes_Collector;
            newLabSample.Lab_Rejection_Comment = labSampleFromModel.Lab_Rejection_Comment;
            newLabSample.Status = "Sent To Testing";

            //try
            //{
            //    DataAccess.AddNewSingleLabSample(labSampleFromModel);
            //}
            //catch(Exception e)
            //{
            //    Debug.WriteLine("Exception e: " + e.InnerException.Message);
            //}
            return newLabSample;
        }

        private static string SetViolationType(LabSample newLabSample)
        {
            ContaminantType newSampleType = DataAccess.GetContaminantTypeFromId((int)newLabSample.fkContaminantTypeId);
            string result = "";
            if (newSampleType.ContaminantCode == "R")
            {
                if ((bool)newLabSample.PesticideResidue.Banned) result = "V";
               else result = DataAccess.SetResidueViolationType((Double.Parse(newLabSample.ConFinal)), (int)newLabSample.fkCommodityId, newLabSample.fkPesticideResidueId);
            }else if (newSampleType.ContaminantCode == "AF")
            {
                result = DataAccess.SetMycotoxinViolationType((Double.Parse(newLabSample.ConFinal)), (int)newLabSample.fkCommodityId, newLabSample.fkMycotoxinId);
            }
            return result;
        }
    }
}