using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBite.BAL.DTOS
{
    public class SaveMealDTO
    {
        public int UserID { get; set; }
        public List<TinyFoodItemDTO>? foodItems { get; set; }
    }
}
