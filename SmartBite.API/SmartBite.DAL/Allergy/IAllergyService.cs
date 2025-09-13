using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartBite.DAL.Models;

namespace SmartBite.DAL.Allergy
{
    public interface IAllergyService
    {

        public List<string> GetUserAllergies(int UserId);

        
        public bool AddUserAllergies(int userId, List<string> allergies);

        public bool UpdateUserAllergies(int UserId, List<string> allergies);

        public bool DeleteUserAllergies(int UserId);

        public bool IsUserHasAllergies(int userId);

        public List<AllergyItemModel> GetUserAllergiesWithHarmfulItems(int userId);

        public List<AllergyIngModel> GetUserAllergiesWithHarmfulIngredients(int userId);


    }
}
