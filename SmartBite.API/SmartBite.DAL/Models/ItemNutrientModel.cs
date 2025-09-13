using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBite.DAL.Models
{
    public class ItemNutrientModel
    {
        public string? Name { get; set; }
        public decimal AmountPerServing { get; set; }
        public string? AmountUnit { get; set; }
    }
}
