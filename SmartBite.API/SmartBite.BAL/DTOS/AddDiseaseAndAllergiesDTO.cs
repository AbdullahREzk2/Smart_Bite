using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBite.BAL.DTOS
{
    public class AddDiseaseAndAllergiesDTO
    {
        public int UserID {  get; set; }
        public DiseaseandAllergyiesDataDTO? input {  get; set; }
    }
}
