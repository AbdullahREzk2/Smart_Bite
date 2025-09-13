using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartBite.BAL.DTOS;
using SmartBite.BAL.FoodTransactions;
using SmartBite.BAL.HealthProfileOperations;
using SmartBite.BAL.MealOperations;
using SmartBite.BAL.OthersServiceManager;
using SmartBite.API.Models;

namespace SmartBite.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OthersController : ControllerBase
    {
        private readonly IOthersServiceManeger _othersServiceManeger;
        private readonly IFoodTransactionManager _foodTransaction;
        private readonly IHealthProfileManager _profileManager;
        public OthersController(IOthersServiceManeger othersServiceManeger , IFoodTransactionManager foodTransaction, IHealthProfileManager profileManager)
        {
            _othersServiceManeger = othersServiceManeger;
            _foodTransaction = foodTransaction;
            _profileManager = profileManager;
        }

        #region Add Rate
        [HttpPut("Add-Rate")]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public ActionResult AddRate(AddRateDTO addRate)
        {
            bool AddedRate = _othersServiceManeger.AddRate(addRate.UserID, addRate.Rate);
            return Ok(AddedRate);
        }
        #endregion

        #region Get Food Items Names
        [HttpGet("Get-AllFoodItems")]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public ActionResult GetNames()
        {
            var Names = _foodTransaction.GetAllFoodItemsNames();
            return Ok(Names);
        }
        #endregion


        #region Update User Image
        [HttpPut("update-image")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUserImage(UploadUserImageDto userImage)
        {
            if (userImage.File == null || userImage.File.Length == 0)
                return BadRequest("No file uploaded.");

            using (var ms = new MemoryStream())
            {
                await userImage.File.CopyToAsync(ms);
                var success = _profileManager.UpdateUserImage(userImage.UserId, ms.ToArray());

                if (success)
                    return Ok(new { message = "Image updated successfully." });
                else
                    return NotFound(new { message = "User not found." });
            }
        }
        #endregion



        #region Get User Image
        [HttpGet("Get-image")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUserImage(int userId)
        {
            var imageData = _profileManager.GetUserImage(userId);

            if (imageData == null)
                return NotFound("Image not found for this user.");

           
            return File(imageData, "image/jpeg");
        }
        #endregion


        #region Delete User Image
        [HttpDelete("Delete-image")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteUserImage(int userId)
        {
            var success = _profileManager.DeleteUserImage(userId);

            if (success)
                return Ok(new { message = "Image deleted successfully." });
            else
                return NotFound(new { message = "User not found." });
        }
        #endregion



    }
}
