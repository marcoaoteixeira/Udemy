using System.Text.Json.Serialization;

namespace MVVMWeatherApp.Models {
    public class Temperature {

        [JsonPropertyName("Metric")]
        public Units Metric { get; set; } = new();

        [JsonPropertyName("Imperial")]
        public Units Imperial { get; set; } = new();
    }
}
