using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBite.BAL.DTOS
{
    public class NutritionDataDTO
    {
        public double? Energy { get; set; }
        public double? Fat { get; set; }
        public double? SaturatedFat { get; set; }
        public double? Carbohydrates { get; set; }
        public double? Sugars { get; set; }
        public double? Fiber { get; set; }
        public double? Proteins { get; set; }
        public double? Salt { get; set; }
    }

}
