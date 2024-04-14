using System.Text.Json.Serialization;

namespace MVVMWeatherApp.Models {
    public class CurrentConditions {

        [JsonPropertyName("LocalObservationDateTime")]
        public DateTime LocalObservationDateTime { get; set; }

        [JsonPropertyName("EpochTime")]
        public int EpochTime { get; set; }

        [JsonPropertyName("WeatherText")]
        public string WeatherText { get; set; }

        [JsonPropertyName("WeatherIcon")]
        public int WeatherIcon { get; set; }

        [JsonPropertyName("HasPrecipitation")]
        public bool HasPrecipitation { get; set; }

        [JsonPropertyName("PrecipitationType")]
        public object PrecipitationType { get; set; }

        [JsonPropertyName("IsDayTime")]
        public bool IsDayTime { get; set; }

        [JsonPropertyName("Temperature")]
        public Temperature Temperature { get; set; } = new();

        [JsonPropertyName("MobileLink")]
        public string MobileLink { get; set; }

        [JsonPropertyName("Link")]
        public string Link { get; set; }
    }
}
