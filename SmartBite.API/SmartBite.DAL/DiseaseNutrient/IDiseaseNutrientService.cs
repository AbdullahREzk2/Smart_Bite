using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using SmartBite.DAL.Models;

namespace SmartBite.DAL.DiseaseNutrient
{
    public interface IDiseaseNutrientService
    {

        public List<NutrientLimitModel> GetDiseaseNutrientLimits(int UserId);

        public void AddDiseaseNutrientLimitsForUser(int userId, List<stNutrientLimitModel> limits);

        public bool DeleteDiseaseNutrientLimitForUser(int UserId);

        public bool UpdateDiseaseNutrientLimitForUser(int UserId, List<stNutrientLimitModel> newLimits);

        public List<NutrientLimitModel> GetDiseaseNutrientLimitsFromVariable(int userId);

        public void UpdateDailyLimits(int userId, List<NutrientLimitModel> RemainingLimits);

        public void AddDiseaseNutrientLimitsToVariable(int userId, List<stNutrientLimitModel> limits);

        public void UpdateDiseaseNutrientLimitsToVariable(int userId, List<stNutrientLimitModel> newLimits);

    }
}
