using SmartBite.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBite.BAL.DTOS
{
    public class MetersDTO
    {
        public decimal? Calories { get; set; }
        public List<NutrientLimitModel>? Limits { get; set; }

    }
}
