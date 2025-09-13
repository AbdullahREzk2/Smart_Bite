using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBite.BAL.DTOS
{
    public class GetProductsReportDTO
    {
       public int UserID { get; set; }
       public string? Barcode { get; set; }
    }
}
