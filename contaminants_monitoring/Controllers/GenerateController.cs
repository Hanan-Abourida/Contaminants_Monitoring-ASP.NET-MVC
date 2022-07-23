using Contaminants_Monitoring.Models;
using Contaminants_Monitoring.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Contaminants_Monitoring.Controllers
{
    public class GenerateController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        // GET: Generate
        public ActionResult SamplesCreation()
        {
            DesignViewModel designViewModel = new DesignViewModel();
            designViewModel.governorates = DataAccess.GetAllGovernorates();
            designViewModel.commodities = DataAccess.getAllCommodities();
            designViewModel.contaminantTypes = DataAccess.GetAllContaminantsTypes();
            designViewModel.cazas = DataAccess.GetAllCazas();
            designViewModel.samplingReasons = DataAccess.GetAllSamplingReasons();
            designViewModel.samplingPlans = DataAccess.GetAllSamplingPlans();
            return View(designViewModel);
        }


        [HttpPost]
        public ActionResult SamplesCreation(DesignViewModel designViewModel)
        {
            int samplesSize = designViewModel.SamplesSize;
            int selectedCommodity = designViewModel.selectedCommodityId;
            int selectedGovernorate = designViewModel.selectedGovernorateId;
            int selectedCaza = designViewModel.selectedCazaId;
            int selectedContaminantType = designViewModel.selectedContaminantTypeId;
            int? selectedSamplingReason = designViewModel.selectedSamplingReasonId;
            int? selectedPlan = designViewModel.selectedSamplingPlanId;

            if(samplesSize <= 0 || selectedCommodity <= 0 || selectedGovernorate <= 0 || selectedCaza <= 0 || selectedContaminantType <= 0 || selectedPlan == null)
            {
                ViewBag.ErrorMessage = "Error in samples generation, some fields are empty!";
                designViewModel.governorates = DataAccess.GetAllGovernorates();
                designViewModel.commodities = DataAccess.getAllCommodities();
                designViewModel.contaminantTypes = DataAccess.GetAllContaminantsTypes();
                designViewModel.cazas = DataAccess.GetAllCazas();
                designViewModel.samplingReasons = DataAccess.GetAllSamplingReasons();
                designViewModel.samplingPlans = DataAccess.GetAllSamplingPlans();
                return View(designViewModel);
            }
            string notes = designViewModel.Notes;

            List<string> identityCodes = new List<string>();
            List<LabSample> generatedLabSamples = new List<LabSample>();
            List<GeneratedSamplesViewModel> generatedSamplesViews = new List<GeneratedSamplesViewModel>();
            
            string governorateCode = DataAccess.GetGovernorateFromId(selectedGovernorate).GovernorateCode;
            string cazaCode = DataAccess.GetCazaFromId(selectedCaza).CazaCode;
            string planCode = DataAccess.GetSamplingPlanFromId((int)selectedPlan).ShortCode;
            // get max number of same governorate and caza
            int maxSize = GetMaxSizeForSpecificGovCaza(governorateCode, cazaCode);
            int inc = maxSize + 1;
            for (int i = 0; i<samplesSize; i++)
            { 
                string identityCode = "";
                if (inc < 10)
                    identityCode = planCode + governorateCode + cazaCode + "00" + inc.ToString();
                else if(inc < 100)
                    identityCode = planCode + governorateCode + cazaCode + "0" + inc.ToString();
                else identityCode = planCode + governorateCode + cazaCode + inc.ToString();
                inc++;
                //create Sample
                LabSample newLabSample = new LabSample();
                newLabSample.SampleNb = identityCode;
                newLabSample.fkCommodityId = selectedCommodity;
                newLabSample.fkGovernorateId = selectedGovernorate;
                newLabSample.fkCazaId = selectedCaza;
                newLabSample.fkContaminantTypeId = selectedContaminantType;
                newLabSample.fkSamplingReasonId = selectedSamplingReason;
                newLabSample.SamplingPlan = selectedPlan;
                newLabSample.Notes_Designer = notes;
                newLabSample.Status = "Generated";
                generatedLabSamples.Add(newLabSample);

                identityCodes.Add(identityCode);

            }
            if (!DataAccess.AddNewLabSample(generatedLabSamples))
            {
                ViewBag.ErrorMessage = "Error in samples generation!";
            }
            else
            {
                designViewModel.generatedSamplesViews = GetGeneratedViewModelFromLabSample(generatedLabSamples);

                //Add to collectionDesign Table
                List<CollectionDesign> collectionDesigns = new List<CollectionDesign>();
                foreach (GeneratedSamplesViewModel model in designViewModel.generatedSamplesViews)
                {
                    CollectionDesign newCollectionDesign = new CollectionDesign();
                    newCollectionDesign.fkLabSampleId = model.fkLabSampleId;
                    newCollectionDesign.GeneratedDate = DateTime.Now.Date;
                    newCollectionDesign.fkDesignerId = User.Identity.GetUserId();
                    collectionDesigns.Add(newCollectionDesign);
                }
                DataAccess.AddNewCollectionDesignList(collectionDesigns);
            }

            designViewModel.governorates = DataAccess.GetAllGovernorates();
            designViewModel.commodities = DataAccess.getAllCommodities();
            designViewModel.contaminantTypes = DataAccess.GetAllContaminantsTypes();
            designViewModel.cazas = DataAccess.GetAllCazas();
            designViewModel.samplingReasons = DataAccess.GetAllSamplingReasons();
            designViewModel.samplingPlans = DataAccess.GetAllSamplingPlans();
            return View(designViewModel);
        }

        private List<GeneratedSamplesViewModel> GetGeneratedViewModelFromLabSample(List<LabSample> labSamples)
        {
            List<GeneratedSamplesViewModel> generatedSamplesViews = new List<GeneratedSamplesViewModel>();
            foreach(LabSample labSample in labSamples)
            {
                GeneratedSamplesViewModel generatedSamplesView = new GeneratedSamplesViewModel();
                generatedSamplesView.caza = labSample.Caza;
                generatedSamplesView.commodity = labSample.Commodity;
                generatedSamplesView.ContaminantType = labSample.ContaminantType;
                generatedSamplesView.governorate = labSample.Governorate;
                generatedSamplesView.Notes = labSample.Notes_Designer;
                generatedSamplesView.fkLabSampleId = DataAccess.GetLabSampleIdFromIdentityCode(labSample.SampleNb);
                generatedSamplesView.SampleIdentityCode = labSample.SampleNb;
                generatedSamplesViews.Add(generatedSamplesView);
            }
            return generatedSamplesViews;
        }

        private int GetMaxSizeForSpecificGovCaza(string governorateCode, string cazaCode)
        {
            int max_code = DataAccess.GetMaxCodeForSpecificGovCaza(governorateCode, cazaCode);
            return max_code; 
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LabSample labSample = DataAccess.GetLabSampleFromId(id);
            if (labSample == null)
            {
                return HttpNotFound();
            }
            CollectorSelectorViewModel collectorSelectorVM = new CollectorSelectorViewModel();
            collectorSelectorVM.collectors = UsersDataAccess.GetCollectors();
            collectorSelectorVM.Laboratories = DataAccess.GetLaboratories();
            collectorSelectorVM.labSampleId = (int)id;
            //GeneratedSamplesViewModel generatedSamplesViewModel = new GeneratedSamplesViewModel();
            //generatedSamplesViewModel.fkLabSampleId = (int)id;
            //generatedSamplesViewModel.commodity = labSample.Commodity;
            //generatedSamplesViewModel.ContaminantType = labSample.ContaminantType;
            //generatedSamplesViewModel.caza = labSample.Caza;
            //generatedSamplesViewModel.governorate = labSample.Governorate;
            //generatedSamplesViewModel.SampleIdentityCode = labSample.SampleNb;
            return View(collectorSelectorVM);
        }

        //[HttpPost]
        //public ActionResult Edit(CollectorSelectorViewModel collectorSelectorVM)
        //{
        //    collectorSelectorVM.collectors = UsersDataAccess.GetCollectors();
        //    collectorSelectorVM.Laboratories = DataAccess.GetLaboratories();
        //    ApplicationUser selectedApplicationUser = UsersDataAccess.GetCollectorFromId(collectorSelectorVM.selectedCollectorId);

        //    int labSampleId = collectorSelectorVM.labSampleId;
        //    if (DataAccess.AddCollectorAndLabToSampleDesign(labSampleId, selectedApplicationUser, collectorSelectorVM.selectedLabId, collectorSelectorVM.DueDate))
        //    {
        //        ViewBag.Message = "Assigned Successfully";
        //        ViewBag.ErrorMessage = "";
        //       // return this.AllSamples();
        //        //List<SampleStatusViewModel> samples = new List<SampleStatusViewModel>();
        //        //samples = DataAccess.GetAllLabSamplesWithDesign();
        //        //return View("AllSamples", samples);
        //    }
        //    else
        //    {
        //        ViewBag.Message = "";
        //        ViewBag.ErrorMessage = "Error in mapping!";
        //    }
               
        //    return View(collectorSelectorVM);
        //}

        [HttpPost]
        public ActionResult DisplayCollectorData(string val)
        {
            if (!string.IsNullOrEmpty(val))
            {
                ApplicationUser selectedCollector = UsersDataAccess.GetCollectorFromId(val);
                return Json(new { Success = "true", Data = new { 
                    Email = selectedCollector.Email,
                    UserName = selectedCollector.UserName,
                    PhoneNumber = selectedCollector.PhoneNumber,
                    Center = selectedCollector.Center
                } });
            }
            return Json(new { Success = "false" });
        }

        [HttpPost]
        public ActionResult DisplayLabData(string val)
        {
            if (!string.IsNullOrEmpty(val))
            {
                Laboratory selectedLab = DataAccess.GetLaboratoryFromId(int.Parse(val));
                return Json(new
                {
                    Success = "true",
                    Data = new
                    {
                        LabName = selectedLab.LaboratoryName,
                        Code = selectedLab.LaboratoryCode,
                        Phone = selectedLab.LaboratoryCode,
                        Email = selectedLab.LaboratoryCode,
                        Address = selectedLab.LaboratoryCode,
                        Mobile = selectedLab.LaboratoryCode
                    }
                });
            }
            return Json(new { Success = "false" });
        }

        [HttpPost]
        public ActionResult DisplaySampleNumbers(string govVal, string cazaVal, string planVal)
        {
            if (!string.IsNullOrEmpty(govVal) && !string.IsNullOrEmpty(cazaVal) && !string.IsNullOrEmpty(planVal))
            {
                Caza selectedCaza = DataAccess.GetCazaFromId(int.Parse(cazaVal));
                Governorate selectedGov = DataAccess.GetGovernorateFromId(int.Parse(govVal));
                SamplingPlan selectedPlan = DataAccess.GetSamplingPlanFromId(int.Parse(planVal));


                //List<string> fromNumbers = DataAccess.GetGeneratedSamplesForGovCazaUnassigned(selectedGov.GovernorateCode, selectedCaza.CazaCode);
                List<string> fromNumbers = DataAccess.GetGeneratedSamplesForGovCazaPlanUnassigned(selectedGov.GovernorateCode, selectedCaza.CazaCode, selectedPlan.ShortCode);
                List<string> toNumbers = fromNumbers;
                return Json(new
                {
                    Success = "true",
                    Data = new
                    {
                        fromList = fromNumbers.ToArray(),
                        toList = toNumbers.ToArray()
                    }
                });
            }
            return Json(new { Success = "false" });
        }

        [HttpGet]
        public ActionResult Map()
        {
            MapRangeViewModel mapRangeViewModel = new MapRangeViewModel();
            mapRangeViewModel.governorates = DataAccess.GetAllGovernorates();
            mapRangeViewModel.cazas = DataAccess.GetAllCazas();
            mapRangeViewModel.listFromNb = new List<string>();
            mapRangeViewModel.listToNb = new List<string>();
            mapRangeViewModel.collectors = UsersDataAccess.GetCollectors();
            mapRangeViewModel.Laboratories = DataAccess.GetLaboratories();
            mapRangeViewModel.samplingPlans = DataAccess.GetAllSamplingPlans();
            //ApplicationUser selectedApplicationUser = UsersDataAccess.GetCollectorFromId(collectorSelectorVM.selectedCollectorId);

            //int labSampleId = collectorSelectorVM.labSampleId;
            return View(mapRangeViewModel);
        }

        [HttpPost]
        public ActionResult Map(MapRangeViewModel mapRangeViewModel)
        {
            mapRangeViewModel.governorates = DataAccess.GetAllGovernorates();
            mapRangeViewModel.cazas = DataAccess.GetAllCazas();
            mapRangeViewModel.collectors = UsersDataAccess.GetCollectors();
            mapRangeViewModel.Laboratories = DataAccess.GetLaboratories();
            mapRangeViewModel.samplingPlans = DataAccess.GetAllSamplingPlans();

            string fromSampleNb_str = mapRangeViewModel.selectedFromNb;
            string toSampleNb_str = mapRangeViewModel.selectedToNb;

            string prefixCode = GetPrefix(mapRangeViewModel.selectedGovernorateId, mapRangeViewModel.selectedCazaId, (int)mapRangeViewModel.selectedSamplingPlanId);

            int fromSampleNb_int = int.Parse(fromSampleNb_str.Substring(prefixCode.Length));
            int toSampleNb_int = int.Parse(toSampleNb_str.Substring(prefixCode.Length));
            List<int> samplesToUpdate = new List<int>();
            List<string> viewSamplesList = new List<string>();
            for(int i = fromSampleNb_int; i <= toSampleNb_int; i++)
            {
                string count = "";
                if (i < 10) count = "00" + i;
                else if (i < 100) count = "0" + i;
                else if(i>=100) count = i.ToString();

                int currentSampleId = DataAccess.GetLabSampleIdFromIdentityCode(prefixCode + count);
                samplesToUpdate.Add(currentSampleId);
                viewSamplesList.Add(prefixCode + count);
            }
            mapRangeViewModel.listFromNb = viewSamplesList;
            mapRangeViewModel.listToNb = viewSamplesList;
            //Update data in List
            ApplicationUser selectedApplicationUser = UsersDataAccess.GetCollectorFromId(mapRangeViewModel.selectedCollectorId);
            int labId = mapRangeViewModel.selectedLabId;
            Nullable<System.DateTime> dueDate = mapRangeViewModel.DueDate;
            Nullable<System.DateTime> startDate = mapRangeViewModel.StartDate;
            foreach (int id in samplesToUpdate)
            {
                if (!DataAccess.AddCollectorAndLabToSampleDesign(id, selectedApplicationUser, labId, dueDate, startDate))
                {
                    ViewBag.Message = "";
                    ViewBag.ErrorMessage = "Error in saving sample: " + id;
                    return View(mapRangeViewModel);
                }
            }
           // await SendEmailNotificationToCollector();
            ViewBag.Message = "Selected range of samples is assigned to collector and lab successfully.\nEmail sent to collector.";
            ViewBag.ErrorMessage = "";
            return View(mapRangeViewModel);
        }

        private string GetPrefix(int govId, int cazaId, int PlanId) 
        {
            
            string govCode = DataAccess.GetGovernorateFromId(govId).GovernorateCode;
            string cazaCode = DataAccess.GetCazaFromId(cazaId).CazaCode;
            string planCode = DataAccess.GetSamplingPlanFromId(PlanId).ShortCode;
            string result = planCode + govCode + cazaCode;
            return result;
        }

        private async Task SendEmailNotificationToCollector()
        {
            var message = new MailMessage();
            message.To.Add(new MailAddress(""));  // replace with valid value 
            message.From = new MailAddress("");  // replace with valid value
            message.Subject = "Collection Notification";
            message.Body = "You were assigned to collect new samples.";
            message.IsBodyHtml = true;


            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "",  // replace with valid value
                    Password = ""  // replace with valid value
                };
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = credential;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                await smtp.SendMailAsync(message);
            }
        }

        public ActionResult AllSamples()
        {
            List<SampleStatusViewModel> samples = new List<SampleStatusViewModel>();
            samples = DataAccess.GetAllLabSamplesWithDesign().OrderByDescending(s => s.labSampleId).ToList();
           
            return View(samples);
        }

        [HttpGet]
        public ActionResult SendEmailToCollector()
        {
            List<ApplicationUser> collectors = new List<ApplicationUser>();
            collectors = UsersDataAccess.GetCollectors();
            NotificationViewModel notificationViewModel = new NotificationViewModel();
            notificationViewModel.allCollectors = collectors;
            return View(notificationViewModel);
        }

        [HttpPost, ActionName("SendEmailToCollector")]
        public async System.Threading.Tasks.Task<ActionResult> SendEmailToCollectorAsync(FormCollection formCollection)
        {
            string[] ids = formCollection["collectorInfoID"].Split(new char[] { ',' });
            foreach (string id in ids)
            {
                ApplicationUser collector = UsersDataAccess.GetCollectorFromId(id);
                //var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
                var message = new MailMessage();
                message.To.Add(new MailAddress(""));  // replace with valid value 
                message.From = new MailAddress("");  // replace with valid value
                message.Subject = "Your email subject";
                message.Body = "this is the body of the message";
                message.IsBodyHtml = true;


                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "",  // replace with valid value
                        Password = ""  // replace with valid value
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    await smtp.SendMailAsync(message);
                    //return RedirectToAction("Sent");
                }
            }
            ViewBag.Message = "Email sent to collector(s)";
            List<ApplicationUser> collectors = new List<ApplicationUser>();
            collectors = UsersDataAccess.GetCollectors();
            NotificationViewModel notificationViewModel = new NotificationViewModel();
            notificationViewModel.allCollectors = collectors;
            return View("SendEmailToCollector", notificationViewModel);
            //return Content("<h3>Email is sent</h3>");
        }

        [HttpGet]
        public ActionResult CollectionIndex()
        {
            string userId = User.Identity.GetUserId();
            List<CollectionViewModel> collectionViews = new List<CollectionViewModel>();
            collectionViews = UsersDataAccess.GetAllLabSamplesForAssignedColletor(userId);
            return View(collectionViews);
        }

        [HttpPost, ActionName("CollectionIndex")]
        public async System.Threading.Tasks.Task<ActionResult> CollectionIndexAsync(FormCollection formCollection)
        {
            //reset view
            List<CollectionViewModel> collectionViews = new List<CollectionViewModel>();
            collectionViews = UsersDataAccess.GetAllLabSamplesForAssignedColletor(User.Identity.GetUserId());

            string[] ids = formCollection["sampleID"].Split(new char[] { ',' });
            List<LabSample> selectedSamples = GetSamplesFromIds(ids);
            bool validateResult = validateToDispatch(selectedSamples);
            if (!validateResult)
            {
                ViewBag.ErrorMessage = "Can not dispatch because some fields are empty.";
                return View(collectionViews);
            }

            bool dispatchResult = DataAccess.DispatchLabSamples(selectedSamples);
           
            if (!dispatchResult)
            {
                ViewBag.Message = "Error saving dispatch data.";
            }
            else
            {
              //  await NotifyLaboratoriesForSamples(selectedSamples);
                ViewBag.Message = "Email successfully sent to lab.";
                collectionViews = UsersDataAccess.GetAllLabSamplesForAssignedColletor(User.Identity.GetUserId());
            }
            return View(collectionViews);
        }

        private bool validateToDispatch(List<LabSample> selectedSamples)
        {
            foreach (LabSample labSample in selectedSamples)
            {
                if (labSample.CommodityState == null ||
                    String.IsNullOrEmpty(labSample.BatchLotNumber) ||
                    String.IsNullOrEmpty(labSample.BrandName) ||
                    String.IsNullOrEmpty(labSample.CollectedQuantity) ||
                    String.IsNullOrEmpty(labSample.ContactName) ||
                    String.IsNullOrEmpty(labSample.ContactPhoneNumber) ||
                    labSample.Country == null ||
                    String.IsNullOrEmpty(labSample.Distributor) ||
                    String.IsNullOrEmpty(labSample.Importer) ||
                    String.IsNullOrEmpty(labSample.Manufacturer) ||
                    labSample.PackagingType == null ||
                    String.IsNullOrEmpty(labSample.PackQuantitySize) ||
                    String.IsNullOrEmpty(labSample.PremiseAddress) ||
                    String.IsNullOrEmpty(labSample.PremiseName) ||
                    String.IsNullOrEmpty(labSample.PremiseType) ||
                    labSample.SampleOrigin == null ||
                    labSample.SamplingDate == null ||
                    String.IsNullOrEmpty(labSample.SamplingTime) ||
                    labSample.SamplingType == null ||
                    String.IsNullOrEmpty(labSample.ShelfLife) ||
                    String.IsNullOrEmpty(labSample.SourceAddress) ||
                    String.IsNullOrEmpty(labSample.SourceContactName) ||
                    String.IsNullOrEmpty(labSample.SourceContactNumber) ||
                    String.IsNullOrEmpty(labSample.SourceName) ||
                    labSample.SourceType == null ||
                    String.IsNullOrEmpty(labSample.StorageConditions))
                    return false;
            }
            return true;
        }


        private List<LabSample> GetSamplesFromIds(string[] ids)
        {
            List<LabSample> labSamples = new List<LabSample>();
            foreach(string i in ids)
            {
                labSamples.Add(DataAccess.GetLabSampleFromId(int.Parse(i)));
            }
            return labSamples;
        }

        public async System.Threading.Tasks.Task<bool> NotifyLaboratoriesForSamples(List<LabSample> selectedSamples)
        {
            
            List<int?> distinctLabs = (from a in selectedSamples
                                where a.fkLaboratoryId != null
                                select a.fkLaboratoryId).Distinct().ToList();

            foreach(int labId in distinctLabs)
            {
                string msgBody = "<h4>Samples Dispatched</h4>";

                string labEmail = "";//MUST BE REPLACED WITH DATAACCESS TO RETREIVE EMAIL FROM ID

                foreach (LabSample labSample in selectedSamples)
                {
                    msgBody = msgBody + "<dl><dt>Sample identity code</dt><dd>" + labSample.SampleNb + "</dd>";
                    msgBody = msgBody + "<dt>Sampling Date</dt><dd>" + labSample.SamplingDate.Value.ToShortDateString() + "</dd>";
                    msgBody = msgBody + "<dt>Sampling Time</dt><dd>" + labSample.SamplingTime + "</dd></dl><br/><hr/>";

                }

                var message = new MailMessage();
                message.To.Add(new MailAddress(labEmail));  // replace with valid value 
                message.From = new MailAddress("");  // replace with valid value
                message.Subject = "Samples Dispatched Notification";
                message.Body = msgBody;
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "",  // replace with valid value
                        Password = ""  // replace with valid value
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    await smtp.SendMailAsync(message);
                }

                //Send Message by SMS
                // await CallWebAPIMessageAsync();

            }
           
            return true;
        }

        private async System.Threading.Tasks.Task<bool> CallWebAPIMessageAsync()
        {
            using (var client = new HttpClient())
            {
                //Send HTTP requests from here.  
                string uri_str = "https://globesms.net/smshub/api.php?username=FoodSafety&password=F@4859&action=sendsms&from=MOA&to=70385645&text=notificationToLab";
                client.BaseAddress = new Uri(uri_str);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync(uri_str);

                return true;
            }
        }


        [HttpGet]
        public ActionResult EditCollection(int? id)
        {
            LabSample labSample = DataAccess.GetLabSampleFromId(id);
            CollectionViewModel collectionViewModel = new CollectionViewModel();
            collectionViewModel.labSample = labSample;
            collectionViewModel.collectorId = UsersDataAccess.GetCollectorIdFromCollectionDesign(labSample.pkLabSampleId);
            collectionViewModel.commodityStates = DataAccess.GetAllCommodityStates(labSample.fkCommodityId);
            collectionViewModel.originCountries = DataAccess.GetAllCountries();
            collectionViewModel.packagingTypes = DataAccess.GetAllPackagingTypes();
            collectionViewModel.premiseTypes = DataAccess.GetAllPremiseTypes();
            collectionViewModel.sourceTypes = DataAccess.GetAllSourceTypes();
            collectionViewModel.sampleOrigins = DataAccess.GetAllSampleOrigins();
            collectionViewModel.samplingPlans = DataAccess.GetAllSamplingPlans();
            collectionViewModel.samplingTypes = DataAccess.GetAllSamplingTypes();

            collectionViewModel.selectedCommodityStateId = labSample.fkCommodityStateId;
            collectionViewModel.selectedOriginCountryId = labSample.fkOriginCountryId;
            collectionViewModel.selectedPackagingTypeId = labSample.fkPackagingTypeId;
            collectionViewModel.selectedPremiseType = labSample.PremiseType;
            collectionViewModel.selectedSampleOriginId = labSample.fkSampleOriginId;
            collectionViewModel.selectedSamplingPlanId = labSample.SamplingPlan;
            collectionViewModel.selectedSamplingReasonId = labSample.fkSamplingReasonId;
            collectionViewModel.selectedSamplingTypeId = labSample.fkSamplingTypeId;
            collectionViewModel.selectedSourceType = labSample.fkSourceTypeId;
            return View(collectionViewModel);
        }

        [HttpGet]
        public ActionResult CollectionDetails()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SaveCollectionData(CollectionViewModel collectionViewModel)
        {
            string userId = collectionViewModel.collectorId;
            // string samplingPlan = formCollection["SamplingPlan"];
            int id = collectionViewModel.labSample.pkLabSampleId;
            if(collectionViewModel.selectedCommodityStateId != 0)
                collectionViewModel.labSample.fkCommodityStateId = collectionViewModel.selectedCommodityStateId;
            //if(collectionViewModel.selectedSamplingReasonId != 0)
            //    collectionViewModel.labSample.fkSamplingReasonId = collectionViewModel.selectedSamplingReasonId;
            if(collectionViewModel.selectedSamplingTypeId != 0)
                collectionViewModel.labSample.fkSamplingTypeId = collectionViewModel.selectedSamplingTypeId;

            collectionViewModel.labSample.PremiseType = collectionViewModel.selectedPremiseType;
            if(collectionViewModel.selectedSampleOriginId != 0)
                collectionViewModel.labSample.fkSampleOriginId= collectionViewModel.selectedSampleOriginId;
            if (collectionViewModel.selectedSourceType != 0)
                collectionViewModel.labSample.fkSourceTypeId = collectionViewModel.selectedSourceType;
            if (collectionViewModel.selectedSourceType != 0)
                collectionViewModel.labSample.fkSourceTypeId = collectionViewModel.selectedSourceType;
            if (collectionViewModel.selectedOriginCountryId != 0)
                collectionViewModel.labSample.fkOriginCountryId = collectionViewModel.selectedOriginCountryId;
            if (collectionViewModel.selectedPackagingTypeId != 0)
                collectionViewModel.labSample.fkPackagingTypeId = collectionViewModel.selectedPackagingTypeId;
            if(collectionViewModel.selectedSamplingPlanId != null && collectionViewModel.selectedSamplingPlanId != 0)
                collectionViewModel.labSample.SamplingPlan = DataAccess.GetSamplingPlanFromId((int)collectionViewModel.selectedSamplingPlanId).pkSamplingPlanId;
            if (DataAccess.saveCollectionData(id, collectionViewModel.labSample))
            {
                List<CollectionViewModel> collectionViews = new List<CollectionViewModel>();
                collectionViews = UsersDataAccess.GetAllLabSamplesForAssignedColletor(userId);
                return RedirectToAction("CollectionIndex", collectionViews);
            }
            else
            {
                ViewBag.ErrorMessage = "Error Saving Fields";
                CollectionViewModel currentView = new CollectionViewModel();
                currentView.labSample = collectionViewModel.labSample;
                currentView.collectorId = userId;

                currentView.commodityStates = DataAccess.GetAllCommodityStates(currentView.labSample.fkCommodityId);
                currentView.originCountries = DataAccess.GetAllCountries();
                currentView.packagingTypes = DataAccess.GetAllPackagingTypes();
                currentView.premiseTypes = DataAccess.GetAllPremiseTypes();
                currentView.sourceTypes = DataAccess.GetAllSourceTypes();
                currentView.sampleOrigins = DataAccess.GetAllSampleOrigins();
                currentView.samplingPlans = DataAccess.GetAllSamplingPlans();
                currentView.samplingTypes = DataAccess.GetAllSamplingTypes();

                currentView.selectedCommodityStateId = currentView.labSample.fkCommodityStateId;
                currentView.selectedOriginCountryId = currentView.labSample.fkOriginCountryId;
                currentView.selectedPackagingTypeId = currentView.labSample.fkPackagingTypeId;
                currentView.selectedPremiseType = currentView.labSample.PremiseType;
                currentView.selectedSampleOriginId = currentView.labSample.fkSampleOriginId;
                currentView.selectedSamplingPlanId = currentView.labSample.SamplingPlan;
                currentView.selectedSamplingReasonId = currentView.labSample.fkSamplingReasonId;
                currentView.selectedSamplingTypeId = currentView.labSample.fkSamplingTypeId;
                currentView.selectedSourceType = currentView.labSample.fkSourceTypeId;
                return View(collectionViewModel);
            }

        }

        public ActionResult ExportFilteredData()
        {
            var result = TempData["SearchLists"];
            return View("CollectionIndex");
        }

        [HttpPost]
        public ActionResult PostFilteredData(Array[] filteredData)
        {
            TempData["SearchLists"] = filteredData;
            //if (!string.IsNullOrEmpty(filteredData))
            //{
            //    TempData["SearchLists"] = filteredData;
            //    return Json(new
            //    {
            //        Success = "true",
            //    });
            //}
            return Json(new { Success = "false" });
        }

        public ActionResult ExportExcel()
        {
            var grid = new GridView();
            grid.DataSource = TempData["SearchLists"];
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=collectionRecords.xls");
            Response.ContentType = "application/ms-excel";

            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return File((byte[])TempData["SearchLists"], Response.ContentType);
        }

        public ActionResult SamplesCalculation()
        {
            string filename = "MOA.FCMS.SAMPLE CALCULATION.xlsm";
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
    }
}