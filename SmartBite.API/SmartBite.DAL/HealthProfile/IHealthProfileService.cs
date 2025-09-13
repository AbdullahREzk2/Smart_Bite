using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartBite.DAL.Models;

namespace SmartBite.DAL.HealthProfile
{
    public interface IHealthProfileService
    {
        public bool AddUserHealthData(int userId, int age, decimal weight, decimal height, string gender, string activityLevel);

        public UserHealthDataModel GetUserHealth(int UserID);

        public UserHealthprofileDataModel GetUserHealthProfileData(int UserID);

        public bool UpdateProfile(int ID, UserHealthprofileDataModel DataModel);

        public int GetTakeAnsolin(int UserID);

        public bool UpdateUserImage(int userId, byte[] imageData);

        public byte[]? GetUserImage(int userId);

        public bool DeleteUserImage(int userId);

    }
}
