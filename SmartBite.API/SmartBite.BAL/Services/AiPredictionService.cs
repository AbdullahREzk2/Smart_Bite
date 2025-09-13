using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using SmartBite.BAL.Services;

public class AiPredictionService : IAiPredictionService
{
    private readonly HttpClient _httpClient;
    private readonly string _fastApiUrl = "https://habibaamr18-class-seg.hf.space/predict";

    public AiPredictionService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<string>> PredictLabelsAsync(byte[] imageContent, string fileName, string contentType)
    {
        using var content = new MultipartFormDataContent();
        var fileContent = new ByteArrayContent(imageContent);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);

        content.Add(fileContent, "file", fileName); // "file" matches FastAPI param name

        var response = await _httpClient.PostAsync(_fastApiUrl, content);

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Failed to call FastAPI: {response.StatusCode}");

        var responseString = await response.Content.ReadAsStringAsync();

        var json = JsonDocument.Parse(responseString);
        var labels = new List<string>();

        if (json.RootElement.TryGetProperty("predicted_labels", out var labelArray))
        {
            foreach (var item in labelArray.EnumerateArray())
            {
                labels.Add(item.GetString()!);
            }
        }

        return labels;
    }
}
