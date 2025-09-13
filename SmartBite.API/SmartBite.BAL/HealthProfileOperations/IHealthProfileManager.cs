using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartBite.BAL.DTOS;
using SmartBite.DAL.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SmartBite.BAL.HealthProfileOperations
{

    public interface IHealthProfileManager
    {
        public bool AddUserDiseaseandAllergies(int UserID, DiseaseandAllergyiesDataDTO data);
        public bool AddUserHealthData(HealthDataDTO data);
        public HomeDataDTO GetUserLimitsAndCalories(int UserID);
        public MetersDTO GetUserOriginalLimits(int UserID);
        public UserHealthprofileDataDTO GetUserHealthProfileData(int UserID);
        public bool UpdateHealthProfile(int UserID , UserHealthprofileDataDTO data);
        public bool IsUserHealthDataCompleted(int UserID);
        public bool UpdateUserImage(int userId, byte[] imageData);
        public byte[]? GetUserImage(int userId);
        public bool DeleteUserImage(int userId);
    }


}
