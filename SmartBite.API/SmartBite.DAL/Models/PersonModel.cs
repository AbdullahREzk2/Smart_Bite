using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBite.DAL.Models
{
    public class PersonModel
    {
        public int UserID { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }

        public int Age { get; set; }
        public decimal Weight { get; set; }
        public decimal Height { get; set; }

        public string? Gender { get; set; }

        public string? ActivityLevel { get; set; }

        

        

        
    }
}
