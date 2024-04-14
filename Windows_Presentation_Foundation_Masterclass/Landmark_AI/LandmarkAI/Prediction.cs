using System.Text.Json.Serialization;

namespace LandmarkAI {
    public class Prediction {

        [JsonPropertyName("probability")]
        public double Probability { get; set; }

        [JsonPropertyName("tagId")]
        public string TagId { get; set; }

        [JsonPropertyName("tagName")]
        public string TagName { get; set; }
    }
}