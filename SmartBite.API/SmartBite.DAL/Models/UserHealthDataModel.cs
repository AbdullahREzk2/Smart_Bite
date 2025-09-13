using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBite.DAL.Models
{
    public class UserHealthDataModel
    {

        public int Age { get; set; }
        public decimal Weight { get; set; }
        public decimal Height { get; set; }
        public string? Gender { get; set; }
        public string? ActivityLevel { get; set; }
        public decimal BMR { get; set; }
        public decimal BMI { get; set; }



    }
}
