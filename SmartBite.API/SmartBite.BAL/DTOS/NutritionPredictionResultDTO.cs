using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBite.BAL.DTOS
{
    public class NutritionPredictionResultDTO
    {
        public decimal Protein { get; set; }
        public decimal SaturatedFat { get; set; }
        public decimal Carbs { get; set; }
        public decimal Sugar { get; set; }
        public decimal Fat { get; set; }
        public decimal Cholesterol { get; set; }
        public decimal Fiber { get; set; }
        public decimal Sodium { get; set; }
    }


}
