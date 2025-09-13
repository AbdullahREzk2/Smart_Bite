using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBite.BAL.DTOS
{
    public class MyAiObjectDTO
    {
       
            public int Age { get; set; }
            public decimal Weight { get; set; }
            public decimal Height { get; set; }
            public string? Gender { get; set; }
            public string? ActivityLevel { get; set; }
            public string? DiseaseName { get; set; }
            public float BMR { get; set; }
            public float BMI { get; set; }
            public int Insolin { get; set; }
            public int Allergy { get; set; }


        }
    }

