
using System.Text;
using System.Text.Json;
using HotelOpt.Application.DTOs;
using HotelOpt.Application.Interfaces;

using Microsoft.Extensions.Configuration;

namespace HotelOpt.Infrastructure.Services;

public class GeminiService:IGeminiService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly string? _apiKey;

    public GeminiService(IHttpClientFactory clientFactory, IConfiguration config)
    {
        _clientFactory = clientFactory;
        _apiKey = config["Gemini:ApiKey"];
        
    }
    public async Task<GeminiInspectionResult> InspectRoom(string photoUrl, string contentType)
    {
        var blobClient = _clientFactory.CreateClient();
        var image = await blobClient.GetByteArrayAsync(photoUrl);
        var converted = Convert.ToBase64String(image);
        var body = new                                                                                                                                                                                                                    
      {                                                                                                                                                                                                                                 
          contents = new[]                                                                                                                                                                                                              
          {                                                                                                                                                                                                                             
              new                                                                                                                                                                                                                       
              {                                                                                                                                                                                                                         
                  parts = new object[]                                                                                                                                                                                                  
                  {                                                                                                                                                                                                                     
                      new { text = "Inspect this hotel room photo. Respond in this exact format: PASSED or FAILED on the first line, then Issues: followed by a description or None" },                                                                                                                                                                                     
                      new                                                                                                                                                                                                               
                      {                                                                                                                                                                                                                 
                          inline_data = new                                                                                                                                                                                             
                          {                                                                                                                                                                                                             
                              mime_type = contentType,                                                                                                                                                                                 
                              data = converted                                                                                                                                                                                     
                          }                                                                                                                                                                                                             
                      }                                                                                                                                                                                                                 
                  }                                                                                                                                                                                                                     
              }                                                                                                                                                                                                                         
          }                                                                                                                                                                                                                             
      };
        var normalized = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
        var geminiClient = _clientFactory.CreateClient("gemini");
        var uri = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-3-flash-preview:generateContent?key={_apiKey}";
        var response = await geminiClient.PostAsync(uri,normalized);
        var raw = await response.Content.ReadAsStringAsync();                                                                                                                                                                             
        if (!response.IsSuccessStatusCode)                                                                                                                                                                                                
            throw new Exception(raw);  
        var data = JsonDocument.Parse(raw);
        var text = data.RootElement.GetProperty("candidates")[0].GetProperty("content").GetProperty("parts")[0]
            .GetProperty("text").GetString();
        if (text is null) return new GeminiInspectionResult(false, "No response from Gemini");
        var lines = text.Split('\n');
        var passed = lines[0].Trim().StartsWith("PASSED");
        var issues = lines.FirstOrDefault(l => l.StartsWith("Issues:"))?.Replace("Issues:", "").Trim();                                                                                                                                   
        return new GeminiInspectionResult(passed, issues);
    }
}