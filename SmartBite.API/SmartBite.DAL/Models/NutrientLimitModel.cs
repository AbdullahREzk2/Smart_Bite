using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBite.DAL.Models
{
    public class NutrientLimitModel
    {
        public string? NutrientName { get; set; }
        public decimal? LimitPerDay { get; set; }
        public string? NutrientUnit { get; set; }
    }
}
