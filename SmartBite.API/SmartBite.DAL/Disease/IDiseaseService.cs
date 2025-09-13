using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartBite.DAL.Models;

namespace SmartBite.DAL.Disease
{
    public interface IDiseaseService
    {
        public string GetUserDisease(int userId);

        public bool AddUserDisease(int UserId, string DiseaseName);

        public bool UpdateUserDisease(int UserId, string NewDiseaseName);

        public bool DeleteUserDisease(int UserId);

        public bool IsUserHasDisease(int userId);

        public bool UpdateUserInsolin(int UserID ,int Value);
    }
}
