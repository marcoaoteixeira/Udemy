using System.Text.Json.Serialization;

namespace MVVMWeatherApp.Models {
    public class Area {

        [JsonPropertyName("ID")]
        public string ID { get; set; }

        [JsonPropertyName("LocalizedName")]
        public string LocalizedName { get; set; }
    }
}
