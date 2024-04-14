using System.Text.Json.Serialization;

namespace LandmarkAI {
    public class LandmarkPrediction {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("project")]
        public string Project { get; set; }

        [JsonPropertyName("iteration")]
        public string Iteration { get; set; }

        [JsonPropertyName("created")]
        public DateTime Created { get; set; }

        [JsonPropertyName("predictions")]
        public IList<Prediction> Predictions { get; set; }
    }
}