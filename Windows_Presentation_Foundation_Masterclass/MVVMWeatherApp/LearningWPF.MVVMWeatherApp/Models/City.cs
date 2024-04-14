using System.Text.Json.Serialization;

namespace MVVMWeatherApp.Models {
    public class City {

        [JsonPropertyName("Version")]
        public int Version { get; set; }

        [JsonPropertyName("Key")]
        public string Key { get; set; }

        [JsonPropertyName("Type")]
        public string Type { get; set; }

        [JsonPropertyName("Rank")]
        public int Rank { get; set; }

        [JsonPropertyName("LocalizedName")]
        public string LocalizedName { get; set; }

        [JsonPropertyName("Country")]
        public Area Country { get; set; } = new();

        [JsonPropertyName("AdministrativeArea")]
        public Area AdministrativeArea { get; set; } = new();

        public string Representation => $"{LocalizedName} / {AdministrativeArea.LocalizedName} / {Country.LocalizedName}";
    }
}
