using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartBite.BAL.DTOS;

namespace SmartBite.BAL.Services
{
    public interface IProductService
    {
        Task<ProductInfoDTO?> GetProductInfoByBarcodeAsync(string barcode, string language = "en");
        public Task<List<string>> GetProductByBarcode(string barcode);

    }
}
