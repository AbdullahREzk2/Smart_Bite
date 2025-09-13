using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBite.BAL.DTOS
{
    public class UpdateMedicineDTO
    {
       public int UserId { get; set; }
       public MedicineDTO? medicine {  get; set; }
    }
}
