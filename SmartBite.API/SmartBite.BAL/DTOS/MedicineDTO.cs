using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBite.BAL.DTOS
{
    public class MedicineDTO
    {
        public int? MedicineID { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public TimeSpan Time { get; set; }
    }
}
