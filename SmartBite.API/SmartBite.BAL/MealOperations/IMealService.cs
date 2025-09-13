using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartBite.BAL.DTOS;

namespace SmartBite.BAL.MealOperations
{
    public interface IMealService
    {
        public string MealReport(List<TinyFoodItemDTO> Items, int UserID);
        public bool Save(List<TinyFoodItemDTO> TinyItems, int UserID);


    }
}
