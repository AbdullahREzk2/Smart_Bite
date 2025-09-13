using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartBite.BAL.DTOS;
using SmartBite.DAL.Models;

namespace SmartBite.BAL.Medication
{
    public interface IMedicationManager
    {

        public bool AddMedicine(int UserId, MedicineDTO medicine);

        public bool DeleteMedicine(int UserId, int MedicineID);

        public bool UpdateMedicine(int UserId, MedicineDTO medicine);

        public List<MedicineDTO> GetUserMedicines(int UserId);

        
    }
}
