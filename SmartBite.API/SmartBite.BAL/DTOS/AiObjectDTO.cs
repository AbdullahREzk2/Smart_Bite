using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SmartBite.BAL.DTOS
{
    public class AiObjectDTO
    {
      
            [JsonPropertyName("RIAGENDR")]
            public int Gender { get; set; }

            [JsonPropertyName("RIDAGEYR")]
            public float Age { get; set; }

            [JsonPropertyName("BMXWT")]
            public float Weight { get; set; }

            [JsonPropertyName("BMXHT")]
            public float Height { get; set; }

            [JsonPropertyName("BMXBMI")]
            public float BMI { get; set; }

            [JsonPropertyName("BPQ020")]
            public int Haypertension { get; set; }

            [JsonPropertyName("BPQ050A")]
            public int Heart { get; set; }

            [JsonPropertyName("DIQ010")]
            public int Diabites { get; set; }

            [JsonPropertyName("obesity")]
            public int Obisty { get; set; }

            [JsonPropertyName("DIQ050")]
            public int Insolin { get; set; }

            [JsonPropertyName("MCQ300A")]
            public int Allergy { get; set; }

            [JsonPropertyName("activity_level")]
            public int ActivityLevel { get; set; }

            [JsonPropertyName("BMR")]
             public float BMR { get; set; }
        }
    }




