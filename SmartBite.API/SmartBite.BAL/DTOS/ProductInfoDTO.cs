using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SmartBite.BAL.Services.ProductService;

namespace SmartBite.BAL.DTOS
{
    public class ProductInfoDTO
    {
        public string? Barcode { get; set; }
        public string? ProductName { get; set; }
        public string? Ingredients { get; set; }
        public NutritionDataDTO? Nutrients { get; set; }
    }
}
