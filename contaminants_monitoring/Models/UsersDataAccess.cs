using Contaminants_Monitoring.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Contaminants_Monitoring.Models
{
    public class UsersDataAccess
    {
        public static ApplicationDbContext context = new ApplicationDbContext();
        public static FoodSafetyDBEntities dBEntities = new FoodSafetyDBEntities();

        public static List<ApplicationUser> GetCollectors()
        {
            string roleId = getIdForSpecificRole("Regional Samples Collector");
            var users = from u in context.Users
                        where u.Roles.Any(r => r.RoleId == roleId)
                        select u;
            return users.ToList();
        }

        private static string getIdForSpecificRole(string roleName)
        {
            var result = from role in context.Roles
                         where role.Name == roleName
                         select role.Id;
            return result.FirstOrDefault();
        }

        public static ApplicationUser GetCollectorFromId(string selectedCollectorId)
        {
            return context.Users.Where(u => u.Id == selectedCollectorId).FirstOrDefault();
        }

        public static List<CollectionViewModel> GetAllLabSamplesForAssignedColletor(string userId)
        {
            List<CollectionViewModel> result = new List<CollectionViewModel>();
            dBEntities = new FoodSafetyDBEntities();
            var temp = dBEntities.CollectionDesigns.Where(d => d.fkCollectorId == userId);
            foreach(var item in temp)
            {
                CollectionViewModel collectionViewModel = new CollectionViewModel();
                collectionViewModel.labSample = dBEntities.LabSamples.Where(s => s.pkLabSampleId == item.fkLabSampleId).FirstOrDefault();
                collectionViewModel.collectorId = userId;
                result.Add(collectionViewModel);
            }
            List<CollectionViewModel> orderedResult = result.OrderByDescending(m => m.labSample.pkLabSampleId).ToList();
            return orderedResult;
        }

        public static string GetCollectorIdFromCollectionDesign(int labSampleId)
        {
            dBEntities = new FoodSafetyDBEntities();
            string result = dBEntities.CollectionDesigns.Where(c => c.fkLabSampleId == labSampleId).FirstOrDefault().fkCollectorId;
            return result;
        }

        public static List<LabSample> GetAllSamplesDispatchedForOneLab(string userId)
        {
            dBEntities = new FoodSafetyDBEntities();
            List<LabSample> result = new List<LabSample>();
            int LabId = context.Users.Where(u => u.Id.Equals(userId)).FirstOrDefault().LabId;
            result = dBEntities.LabSamples.Where(m => m.fkLaboratoryId == LabId && m.Status == "Dispatched").ToList();
            return result;
        }
        public static List<LabSample> initInventoryWithSent(string userId)
        {
            dBEntities = new FoodSafetyDBEntities();
            List<LabSample> result = new List<LabSample>();
            int LabId = context.Users.Where(u => u.Id.Equals(userId)).FirstOrDefault().LabId;
            result = dBEntities.LabSamples.Where(m => m.fkLaboratoryId == LabId && m.Status == "Sent To Testing").ToList();
            return result;
        }

        public static List<LabSample> initInventoryWithAll(string userId)
        {
            dBEntities = new FoodSafetyDBEntities();
            List<LabSample> result = new List<LabSample>();
            int LabId = context.Users.Where(u => u.Id.Equals(userId)).FirstOrDefault().LabId;
            result = dBEntities.LabSamples.Where(m => m.fkLaboratoryId == LabId && m.Status != "Generated").ToList();
            return result;
        }

        public static List<LabSample> initInventoryWithRejected(string userId)
        {
            dBEntities = new FoodSafetyDBEntities();
            List<LabSample> result = new List<LabSample>();
            int LabId = context.Users.Where(u => u.Id.Equals(userId)).FirstOrDefault().LabId;
            result = dBEntities.LabSamples.Where(m => m.fkLaboratoryId == LabId && m.Status != "Rejected").ToList();
            return result;
        }

        public static ApplicationUser GetCollectorFromSampleId(int sampleId)
        {
            var collectionDesigns = dBEntities.CollectionDesigns.Where(d => d.fkLabSampleId == sampleId).FirstOrDefault();
            string collectorId = "";
            if (collectionDesigns != null)
            {
                collectorId = collectionDesigns.fkCollectorId;
            }
            return context.Users.Where(u => u.Id == collectorId).FirstOrDefault(); 
        }
    }
}