using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBite.BAL.DTOS
{
    public class TinyFoodItemDTO
    {
        public string? Name { get; set; }
        public decimal ServingSize { get; set; }
        public string? ServingUnit { get; set; }
    }
}
