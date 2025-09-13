using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartBite.BAL.DTOS;
using SmartBite.BAL.ProductOperations;
using SmartBite.BAL.ReportManeger;
using SmartBite.BAL.Services;

namespace SmartBite.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductManager _productManager;

        public ProductController(IProductManager productManager)
        {
            _productManager = productManager;
        }


        #region Get product Report
        [HttpGet("Get-ProductData")]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<ActionResult> GetProductsReport(int UserID ,string Barcode)
        {
            var ProductReport = await _productManager.GetProductReport(UserID,Barcode!);
            return Ok(ProductReport);
        }
        #endregion


    }
}
