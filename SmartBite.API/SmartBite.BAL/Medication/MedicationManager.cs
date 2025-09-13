using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SmartBite.BAL.DTOS;
using SmartBite.DAL.Medication;
using SmartBite.DAL.Models;

namespace SmartBite.BAL.Medication
{
    public class MedicationManager : IMedicationManager
    {
        private readonly IMapper _mapper;
        private readonly IMedicationService _medicationService;

        public MedicationManager(IMapper mapper,IMedicationService medicationService)
        {
            _mapper = mapper;
            _medicationService = medicationService;
        }


        public bool AddMedicine(int UserId, MedicineDTO medicine)
        {
            return _medicationService.AddMedicine(UserId,_mapper.Map<MedicineModel>(medicine));
        }

        public bool DeleteMedicine(int UserId, int MedicineID)
        {
           return _medicationService.DeleteMedicine(UserId, MedicineID);
        }

        public List<MedicineDTO> GetUserMedicines(int UserId)
        {
            return _mapper.Map<List<MedicineDTO>>(_medicationService.GetUserMedicines(UserId));
        }

        public bool UpdateMedicine(int UserId, MedicineDTO medicine)
        {
           
            return _medicationService.UpdateMedicine(UserId, _mapper.Map<MedicineModel>(medicine));
        }



    }
}
