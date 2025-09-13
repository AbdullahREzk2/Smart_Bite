using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBite.DAL.Models
{
    public class MedicineModel
    {
        public int? MedicineID { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public TimeSpan Time { get; set; }
    }
}
