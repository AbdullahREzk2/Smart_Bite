using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBite.BAL.DTOS
{
    public class NutrientResultDTO
    {
        public string? NutrientName { get; set; }
        public decimal LimitPerDay { get; set; }
    }

}
