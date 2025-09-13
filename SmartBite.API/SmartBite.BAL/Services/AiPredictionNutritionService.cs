using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using SmartBite.BAL.DTOS;

public class AiPredictionNutritionService
{
    private readonly HttpClient _httpClient;

    public AiPredictionNutritionService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<NutrientResultDTO>> PredictNutrientsAsync(AiObjectDTO input)
    {
        var response = await _httpClient.PostAsJsonAsync("/predict_all", input);
        response.EnsureSuccessStatusCode();

        var resultString = await response.Content.ReadAsStringAsync();

        // The FastAPI returns a plain text result. So parse it.
        var results = new List<NutrientResultDTO>();

        var lines = resultString.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        foreach (var line in lines)
        {
            var parts = line.Split(':');
            if (parts.Length == 2)
            {
                var name = parts[0].Trim();
                if (double.TryParse(parts[1].Trim(), out double value))
                {
                    // Re-round to 2 decimals for extra safety
                    results.Add(new NutrientResultDTO
                    {
                        NutrientName = name,
                        LimitPerDay = Convert.ToDecimal(Math.Round(value, 1))
                    });
                }
            }
        }

        return results;
    }


}
