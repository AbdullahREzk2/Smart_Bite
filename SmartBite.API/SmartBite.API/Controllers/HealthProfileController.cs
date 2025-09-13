using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartBite.BAL.DTOS;
using SmartBite.BAL.HealthProfileOperations;
using SmartBite.BAL.Services;

namespace SmartBite.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthProfileController : ControllerBase
    {
        private readonly IHealthProfileManager _healthProfileManager;

        public HealthProfileController(IHealthProfileManager healthProfileManager)
        {
            _healthProfileManager = healthProfileManager;
        }



        #region Add Health Data
        [HttpPost("AddHealthData")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult AddDiseaseAndAllergies(HealthDataDTO healthdata)
        {
            var result = _healthProfileManager.AddUserHealthData(healthdata);
            return Ok(result);
        }
        #endregion

        #region Get Limits , Calories and Name

        [HttpGet("GetHomeData")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetLimitsAndCalories(int userId)
        {
            var result = _healthProfileManager.GetUserLimitsAndCalories(userId);
            return Ok(result);
        }
        #endregion



        #region Get Original Limits

        [HttpGet("GetOriginalLimits")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetOriginalLimitsForUser(int userId)
        {
            var result = _healthProfileManager.GetUserOriginalLimits(userId);
            return Ok(result);
        }
        #endregion




        #region Get Health profile Data 

        [HttpGet("Get-HealthProfile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult GetProfileHealthData(int userId)
        {
            return Ok(_healthProfileManager.GetUserHealthProfileData(userId));
        }
        #endregion

        #region Update Profile

        [HttpPut("Update-Profile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult UpdateProfile(UpdateProfileDTO updateProfile)
        {
           bool Updated = _healthProfileManager.UpdateHealthProfile(updateProfile.UserID, updateProfile. Data!);
           return Ok(Updated);
        }
        #endregion


        #region Check is health data complete
        [HttpGet("IsHealthDataComplete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult IsHealthDataComplete(int UserID)
        {
            var result = _healthProfileManager.IsUserHealthDataCompleted(UserID);
            return Ok(result);
        }
        #endregion
    }
}
