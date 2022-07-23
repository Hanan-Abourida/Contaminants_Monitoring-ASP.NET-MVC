using Contaminants_Monitoring.Models;
using Contaminants_Monitoring.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Contaminants_Monitoring.Controllers
{
    public class LibraryController : Controller
    {
        // GET: Library
        public ActionResult ContaminantsList()
        {
            return View();
        }

        public ActionResult PesticidesResidues()
        {
            List<PesticideMRLViewModel> allMRLs_VM = new List<PesticideMRLViewModel>();
            allMRLs_VM = DataAccess.GetAllPesticidesAndMRLs();
            return View(allMRLs_VM);
        }

        


        public ActionResult MyCotoxins()
        {
            List<MLViewModel> allMLs_VM = new List<MLViewModel>();
            allMLs_VM = DataAccess.GetAllMLs();
            return View(allMLs_VM);
        }

        public ActionResult OpenPesticidesPage(int id)
        {
            PesticideResidue currentPesticide = DataAccess.GetPesticideResidueFromId(id);
            return View("PesticideResidue",currentPesticide);
        }

        public ActionResult OpenMycotoxinPage(int id)
        {
            Mycotoxin mycotoxin = DataAccess.GetMycotoxinFromId(id);
            return View("Mycotoxin", mycotoxin);
        }

        public ActionResult VeterinaryResidues()
        {
            List<VetMRLModel> allVets_VM = new List<VetMRLModel>();
            allVets_VM = DataAccess.GetAllVetAndMRLs();
            return View(allVets_VM);
        }

        public ActionResult Gov_Caza()
        {
            GovCazaModelView govCazaModelView = new GovCazaModelView();
            govCazaModelView.cazas = DataAccess.GetAllCazas();
            govCazaModelView.governorates = DataAccess.GetAllGovernorates();
            return View(govCazaModelView);
        }

        //Sampling Plan CRUD
        public ActionResult SamplingPlansList()
        {
            List<SamplingPlan> samplingPlans = new List<SamplingPlan>();
            FoodSafetyDBEntities dBEntities = new FoodSafetyDBEntities();
            samplingPlans = dBEntities.SamplingPlans.ToList();
            return View(samplingPlans);
        }

        [HttpGet]
        public ActionResult SamplingPlanCreate()
        {
            SamplingPlan samplingPlan = new SamplingPlan();
            int incrementedMax = DataAccess.GetMaxFromMPSamplingPlans() + 1;
            samplingPlan.ShortCode = "MP" + incrementedMax;
            return View(samplingPlan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SamplingPlanCreate([Bind(Include = "pkSamplingPlanId,SamplingPlanCode,SamplingPlanDesc,ShortCode")] SamplingPlan samplingPlan)
        {
            FoodSafetyDBEntities dBEntities = new FoodSafetyDBEntities();
            if (ModelState.IsValid)
            {
                dBEntities.SamplingPlans.Add(samplingPlan);
                dBEntities.SaveChanges();
                return RedirectToAction("SamplingPlansList");
            }
            return View(samplingPlan);
        }

        // GET: LabSamples/Delete/5
        public ActionResult DeleteSamplingPlan(int? id)
        {
            FoodSafetyDBEntities dBEntities = new FoodSafetyDBEntities();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SamplingPlan samplingPlan = dBEntities.SamplingPlans.Find(id);
            if (samplingPlan == null)
            {
                return HttpNotFound();
            }
            return View(samplingPlan);
        }

        // POST: LabSamples/Delete/5
        [HttpPost, ActionName("DeleteSamplingPlan")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSamplingPlan(int id)
        {
            FoodSafetyDBEntities dBEntities = new FoodSafetyDBEntities();
            SamplingPlan samplingPlan = dBEntities.SamplingPlans.Find(id);
            dBEntities.SamplingPlans.Remove(samplingPlan);
            dBEntities.SaveChanges();
            return RedirectToAction("SamplingPlansList");
        }

        public ActionResult EditSamplingPlan(int? id)
        {
            FoodSafetyDBEntities dBEntities = new FoodSafetyDBEntities();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SamplingPlan samplingPlan = dBEntities.SamplingPlans.Find(id);
            if (samplingPlan == null)
            {
                return HttpNotFound();
            }
           
            return View(samplingPlan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSamplingPlan([Bind(Include = "pkSamplingPlanId,SamplingPlanCode,SamplingPlanDesc,ShortCode")] SamplingPlan samplingPlan)
        {
            FoodSafetyDBEntities dBEntities = new FoodSafetyDBEntities();
            if (ModelState.IsValid)
            {
                dBEntities.Entry(samplingPlan).State = EntityState.Modified;
                dBEntities.SaveChanges();
                return RedirectToAction("SamplingPlansList");
            }
            return View(samplingPlan);
        }
            
        public ActionResult AddMRLValue()
        {
            PesticideMRLViewModel pesticideMRLViewModel = new PesticideMRLViewModel();
            pesticideMRLViewModel.commoditiesList = DataAccess.getAllCommodities();
            pesticideMRLViewModel.pesticidesList = DataAccess.getAllPesticideResidues();
            pesticideMRLViewModel.units = DataAccess.GetAllUOMs();
            return View(pesticideMRLViewModel);
        }

        [HttpPost]
        public ActionResult AddMRLValue(PesticideMRLViewModel pesticideMRLViewModel)
        {
            if (pesticideMRLViewModel.mrl == null || pesticideMRLViewModel.selectedCommodityID == 0 || pesticideMRLViewModel.selectedResID==0 || pesticideMRLViewModel.selectedUOM == 0)
            {
                pesticideMRLViewModel.commoditiesList = DataAccess.getAllCommodities();
                pesticideMRLViewModel.pesticidesList = DataAccess.getAllPesticideResidues();
                pesticideMRLViewModel.units = DataAccess.GetAllUOMs();
                ViewBag.ErrorMessage = "Save failed! One field or more is empty.";
                return View(pesticideMRLViewModel);
            }

            if (DataAccess.AddNewMRL((double)pesticideMRLViewModel.mrl, (int)pesticideMRLViewModel.selectedUOM, pesticideMRLViewModel.selectedCommodityID, pesticideMRLViewModel.selectedResID))
            {
                ViewBag.Message = "New MRL value added successfully.";
            }
            List<PesticideMRLViewModel> allMRLs_VM = new List<PesticideMRLViewModel>();
            allMRLs_VM = DataAccess.GetAllPesticidesAndMRLs();
            return RedirectToAction("PesticidesResidues", allMRLs_VM);
        }

        [HttpGet]
        public ActionResult EditPRMRLValue(int? id)
        {
            PesticideMRLViewModel pesticideMRLViewModel = new PesticideMRLViewModel();
            FoodSafetyDBEntities dBEntities = new FoodSafetyDBEntities();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PestResMRL PRmrl = dBEntities.PestResMRLs.Find(id);
            if (PRmrl == null)
            {
                return HttpNotFound();
            }
            pesticideMRLViewModel = DataAccess.GetSinglePesticideAndMRL(id);
            return View(pesticideMRLViewModel);
        }

        [HttpPost]
        public ActionResult EditPRMRLValue([Bind(Include = "pkPRMRLId,mrl")] PesticideMRLViewModel pesticideMRLViewModel)
        {
                FoodSafetyDBEntities dBEntities = new FoodSafetyDBEntities();
                PestResMRL PRmrl = dBEntities.PestResMRLs.Find(pesticideMRLViewModel.pkPRMRLId);
                PRmrl.MRL = (double)pesticideMRLViewModel.mrl;
                if (ModelState.IsValid)
                {
                    dBEntities.Entry(PRmrl).State = EntityState.Modified;
                    dBEntities.SaveChanges();
                }
            return RedirectToAction("PesticidesResidues");
        }

        [HttpGet]
        public ActionResult EditVetMRL(int? id)
        {
            VetMRLModel vetMRLModel = new VetMRLModel();
            FoodSafetyDBEntities dBEntities = new FoodSafetyDBEntities();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VetResMRL vetResMRL = dBEntities.VetResMRLs.Find(id);
            if (vetResMRL == null)
            {
                return HttpNotFound();
            }
            vetMRLModel = DataAccess.GetSingleVetAndMRL(id);
            return View(vetMRLModel);
        }

        [HttpPost]
        public ActionResult EditVetMRL([Bind(Include = "pkVetMRLId,mrl")] VetMRLModel vetMRLModel)
        {
            FoodSafetyDBEntities dBEntities = new FoodSafetyDBEntities();
            VetResMRL vetResMRL = dBEntities.VetResMRLs.Find(vetMRLModel.pkVetMRLId);
            vetResMRL.MRL = (double)vetMRLModel.mrl;
            if (ModelState.IsValid)
            {
                dBEntities.Entry(vetResMRL).State = EntityState.Modified;
                dBEntities.SaveChanges();
            }
            return RedirectToAction("VeterinaryResidues");
        }


    }
}