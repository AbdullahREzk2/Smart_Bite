using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartBite.DAL.Models;

namespace SmartBite.BAL.DTOS
{


    public class LimitsAndCaloriesDTO
    {

        public decimal? Calories {  get; set; }

        public List<NutrientLimitModel>? Limits {  get; set; }  
        

    }

    
}
