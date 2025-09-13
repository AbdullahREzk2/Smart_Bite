using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartBite.BAL.DTOS;
using SmartBite.BAL.FoodTransactions;
using SmartBite.BAL.MealOperations;
using SmartBite.BAL.ReportManeger;
using SmartBite.BAL.Services;

namespace SmartBite.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MealController : ControllerBase
    {
        private readonly IAiPredictionService _predictionService;
        private readonly IFoodTransactionManager _foodTransaction;
        private readonly IMealService _mealService;

        public MealController(IAiPredictionService predictionService , IFoodTransactionManager foodTransaction , IMealService mealService)
        {
            _predictionService = predictionService;
            _foodTransaction = foodTransaction;
            _mealService = mealService;
        }


        #region analyzaMeal

        [HttpPost("analyze-photo")]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<IActionResult> AnalyzePhoto(IFormFile photo)
        {
            if (photo == null || photo.Length == 0)
                return BadRequest("No image uploaded");

            using var memoryStream = new MemoryStream();
            await photo.CopyToAsync(memoryStream);
            var imageBytes = memoryStream.ToArray();

            var predictions = await _predictionService.PredictLabelsAsync(
                imageBytes,
                photo.FileName,
                photo.ContentType
            );

            var Items = _foodTransaction.GetTinyItemsFromStringList( predictions );

            return Ok(Items);
        }
        #endregion

        #region MealReport
        [HttpPost("GetMealReport")]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public IActionResult GetMealReport(GetMealReportDTO getMealReport)
        {

            string Report = _mealService.MealReport(getMealReport.foodItems!, getMealReport. UserID);
            return Ok(Report);

        }

        #endregion


        #region MealSave
        [HttpPost("Save")]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public IActionResult SaveMeal(SaveMealDTO saveMeal)
        {

            bool saved = _mealService.Save(saveMeal.foodItems!, saveMeal.UserID);  
            return Ok(saved);

        }

        #endregion


    }
}
