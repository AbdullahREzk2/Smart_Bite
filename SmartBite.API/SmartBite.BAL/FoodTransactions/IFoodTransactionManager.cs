using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartBite.BAL.DTOS;
using SmartBite.DAL.Models;

namespace SmartBite.BAL.FoodTransactions
{
    public interface IFoodTransactionManager
    {
        public List<TinyFoodItemModel> GetTinyItemsFromStringList(List<string> RecognizedItems);

        public List<FoodItemModel> GetFoodItemsWithEditedSize(List<TinyFoodItemDTO> EditedtinyFoodItems);

        public List<string> GetAllFoodItemsNames();
    }
}
