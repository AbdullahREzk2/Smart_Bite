using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SmartBite.BAL.DTOS;

namespace SmartBite.BAL.Services
{
    public class ProductService:IProductService
    {


            private readonly HttpClient _httpClient;
            private readonly ILogger<ProductService> _logger;

            public ProductService(HttpClient httpClient, ILogger<ProductService> logger)
            {
                _httpClient = httpClient;
                _httpClient.BaseAddress = new Uri("https://world.openfoodfacts.org/api/v2/");
                _logger = logger;
            }

            public async Task<ProductInfoDTO?> GetProductInfoByBarcodeAsync(string barcode, string language = "en")
            {
                try
                {
                    var response = await _httpClient.GetAsync($"product/{barcode}?lc={language}");
                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogWarning("Failed to retrieve product data for barcode {Barcode}. Status: {Status}",
                            barcode, response.StatusCode);
                        return null;
                    }

                    var jsonResponse = await response.Content.ReadFromJsonAsync<JsonElement>();

                    if (jsonResponse.GetProperty("status").GetInt32() != 1)
                    {
                        _logger.LogWarning("Product not found for barcode {Barcode}", barcode);
                        return null;
                    }

                    var product = jsonResponse.GetProperty("product");

                    var productInfo = new ProductInfoDTO
                    {
                        Barcode = barcode,
                        ProductName = GetStringPropertyOrDefault(product, "product_name"),
                        Ingredients = GetStringPropertyOrDefault(product, "ingredients_text"),
                        Nutrients = new NutritionDataDTO()
                    };

                    if (product.TryGetProperty("nutriments", out var nutriments))
                    {
                        productInfo.Nutrients.Energy = GetDoublePropertyOrDefault(nutriments, "energy-kcal_100g");
                        productInfo.Nutrients.Fat = GetDoublePropertyOrDefault(nutriments, "fat_100g");
                        productInfo.Nutrients.SaturatedFat = GetDoublePropertyOrDefault(nutriments, "saturated-fat_100g");
                        productInfo.Nutrients.Carbohydrates = GetDoublePropertyOrDefault(nutriments, "carbohydrates_100g");
                        productInfo.Nutrients.Sugars = GetDoublePropertyOrDefault(nutriments, "sugars_100g");
                        productInfo.Nutrients.Fiber = GetDoublePropertyOrDefault(nutriments, "fiber_100g");
                        productInfo.Nutrients.Proteins = GetDoublePropertyOrDefault(nutriments, "proteins_100g");
                        productInfo.Nutrients.Salt = GetDoublePropertyOrDefault(nutriments, "salt_100g");
                    }

                    return productInfo;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error retrieving product information for barcode {Barcode}", barcode);
                    throw;
                }
            }

            private string? GetStringPropertyOrDefault(JsonElement element, string propertyName)
            {
                return element.TryGetProperty(propertyName, out var property)
                    ? property.GetString()
                    : string.Empty;
            }

            private double? GetDoublePropertyOrDefault(JsonElement element, string propertyName)
            {
                return element.TryGetProperty(propertyName, out var property) && property.ValueKind != JsonValueKind.Null
                    ? property.TryGetDouble(out var value) ? value : null
                    : null;
            }

            public async Task<List<string>> GetProductByBarcode(string barcode)
            {
                _logger.LogInformation("Received request for barcode {Barcode}", barcode);

                var productInfo = await GetProductInfoByBarcodeAsync(barcode);

                if (productInfo == null)
                {
                    _logger.LogWarning("No product found for barcode {Barcode}", barcode);
                    return new List<string>();
                }

                if (string.IsNullOrWhiteSpace(productInfo.Ingredients))
                {
                    _logger.LogWarning("No ingredients found for barcode {Barcode}", barcode);
                    return new List<string>();
                }

                var ingredientsList = productInfo.Ingredients
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(i => i.Trim())
                    .Where(i => !string.IsNullOrWhiteSpace(i))
                    .ToList();

                return ingredientsList;
            }



    }
}

