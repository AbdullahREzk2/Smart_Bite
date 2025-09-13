using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBite.BAL.ProductOperations
{
    public interface IProductManager
    {
        public Task<string> GetProductReport(int UserID, string Barcode);
    }
}
