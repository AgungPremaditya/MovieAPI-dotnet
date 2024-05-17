using Microsoft.AspNetCore.Routing;
using MovieAPI_dotnet.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace MovieAPI_dotnet.Services
{
    public interface IExternalApiService
    {
        Task<List<Series>> GetHttpAsync();
    }
    public class ExternalApiService : IExternalApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _bearerToken;


        public ExternalApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _bearerToken = "BEARER_TOKEN";
        }

        public async Task<List<Series>> GetHttpAsync() 
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _bearerToken);
            var response = await _httpClient.GetAsync("https://animeschedule.net/api/v3/timetables/sub?tz=Asia/Jakarta");
            response.EnsureSuccessStatusCode();

            var responseStream = await response.Content.ReadAsStreamAsync();
            using var jsonDocument = await JsonDocument.ParseAsync(responseStream);
            var root = jsonDocument.RootElement;

            var series = new List<Series>();

            foreach (var element in root.EnumerateArray())
            {
               var serial =  new Series
                {
                    Title = element.GetProperty("title").GetString(),
                    Status = element.GetProperty("status").GetString(),
                };

                series.Add(serial);
            }

            return series;
        }
    }
}
