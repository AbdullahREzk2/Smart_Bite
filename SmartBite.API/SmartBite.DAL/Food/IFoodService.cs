using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartBite.DAL.Models;

namespace SmartBite.DAL.Food
{
    public interface IFoodService
    {

        public FoodItemModel GetFoodItemByName(string name);

        public List<ItemNutrientModel> GetFoodItemNutrients(string name);

       
        public TinyFoodItemModel GetTinyItemByName(string Name);

        public List<string> GetAllFoodItemsNames();

    }
}
