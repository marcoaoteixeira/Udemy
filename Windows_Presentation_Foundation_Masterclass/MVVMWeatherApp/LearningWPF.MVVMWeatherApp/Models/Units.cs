using System.Text.Json.Serialization;

namespace MVVMWeatherApp.Models {
    public class Units {

        [JsonPropertyName("Value")]
        public int Value { get; set; }

        [JsonPropertyName("Unit")]
        public string Unit { get; set; }

        [JsonPropertyName("UnitType")]
        public int UnitType { get; set; }
    }
}
