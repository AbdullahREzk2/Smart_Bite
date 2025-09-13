using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBite.BAL.DTOS
{
    public class DiseaseandAllergyiesDataDTO
    {
        public DiseseObjectDTO? diseseObject { get; set; }

        public List<string>? Allergies { get; set; }

    }
}
