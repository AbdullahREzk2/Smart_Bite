using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartBite.DAL.Models;

namespace SmartBite.DAL.Medication
{
    public interface IMedicationService
    {

        public bool AddMedicine(int UserId, MedicineModel medicine);

        public bool DeleteMedicine(int UserId, int MedicineID);

        public bool UpdateMedicine(int UserId, MedicineModel medicine);

        public List<MedicineModel> GetUserMedicines(int UserId);
    }
}
