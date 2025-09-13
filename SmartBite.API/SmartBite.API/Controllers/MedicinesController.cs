using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartBite.BAL.DTOS;
using SmartBite.BAL.Medication;

namespace SmartBite.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicinesController : ControllerBase
    {
        private readonly IMedicationManager _medicationManager;

        public MedicinesController(IMedicationManager medicationManager)
        {
            _medicationManager = medicationManager;
        }

        #region GetUserMedicines
        [HttpGet("Get-Medicines")]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public ActionResult GetUserMedinces(int userId)
        {
           var Data = _medicationManager.GetUserMedicines(userId);
            return Ok(Data);
        }
        #endregion

        #region AddMedicine
        [HttpPost("Add-Medicins")]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public ActionResult AddMedicine(AddMedicineDTO addMedicine)
        {
            var MedicineID = _medicationManager.AddMedicine(addMedicine.UserId, addMedicine.medicine!);
            return Ok(MedicineID);
        }
        #endregion

        #region UpdateMedicine
        [HttpPut("Update-Medicine")]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public ActionResult UpdateMedicine(UpdateMedicineDTO updateMedicine)
        {
            bool Updated = _medicationManager.UpdateMedicine(updateMedicine.UserId, updateMedicine.medicine!);
            return Ok(Updated);
        }
        #endregion

        #region deleteMedicine
        [HttpDelete("Delete-Medicine")]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public ActionResult DeleteMedicine(DeleteMedicine deleteMedicine)
        {
            bool Deleted = _medicationManager.DeleteMedicine(deleteMedicine.UserId, deleteMedicine.MedicineID);
            return Ok(Deleted);
        }
        #endregion

    }
}
