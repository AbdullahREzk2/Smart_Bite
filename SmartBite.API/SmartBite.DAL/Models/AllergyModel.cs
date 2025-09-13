using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBite.DAL.Models
{
    public class AllergyModel
    {
        public string? Name { get; set; }
        public List<string>? HarmfulItems { get; set; }
    }
}
