using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartBite.BAL.ReportManeger;
using SmartBite.BAL.Services;
using SmartBite.DAL.Allergy;

namespace SmartBite.BAL.ProductOperations
{
    public class ProductManager : IProductManager
    {
        private readonly IReportService _reportService;
        private readonly IProductService _productService;
        private readonly IAllergyService _allergyService;

        public ProductManager(IReportService reportService, IProductService productService, IAllergyService allergyService)
        {
            _reportService = reportService;
            _productService = productService;
            _allergyService = allergyService;
        }

        public async Task <string> GetProductReport(int UserID, string Barcode)
        {
            var Ingerediants = await _productService.GetProductByBarcode(Barcode);
            var HarmfulIngerediants = _allergyService.GetUserAllergiesWithHarmfulIngredients(UserID);

            if (Ingerediants == null || !Ingerediants.Any())
            {
                return " Product Not Found !";
            }
            
            return _reportService.GenerateProductAllergyReport(Ingerediants, HarmfulIngerediants);

        }



    }
}
