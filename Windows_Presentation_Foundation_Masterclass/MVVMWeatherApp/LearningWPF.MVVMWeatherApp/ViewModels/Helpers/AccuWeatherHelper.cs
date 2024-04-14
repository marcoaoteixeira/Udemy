using System.Net.Http;
using System.Text.Json;
using MVVMWeatherApp.Models;

namespace MVVMWeatherApp.ViewModels.Helpers {
    public class AccuWeatherHelper {
        private const string API_KEY = "aV2rw5G7FFoJb5zAnRnWiZmTPVeoj8mh";
        private const string BASE_URL = "https://dataservice.accuweather.com";

        private const string AUTOCOMPLETE_API = $"{BASE_URL}/locations/v1/cities/autocomplete?apikey={API_KEY}&q={{0}}";
        private const string CURRENT_CONDITIONS_API = $"{BASE_URL}/currentconditions/v1/{{0}}?apikey={API_KEY}";

        public static async Task<City[]> GetCitiesAsync(string term) {
            var url = string.Format(AUTOCOMPLETE_API, term);

            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);
            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<City[]>(responseContent);

            return result ?? [];
        }

        public static async Task<CurrentConditions?> GetCurrentConditionsAsync(string cityKey) {
            var url = string.Format(CURRENT_CONDITIONS_API, cityKey);

            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);
            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<CurrentConditions[]>(responseContent);

            return (result ?? []).SingleOrDefault();
        }
    }
}
