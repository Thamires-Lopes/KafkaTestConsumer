using System.Text.Json.Serialization;

namespace KafkaTestConsumer.Models
{
    public class User
    {
        [JsonPropertyName("Id")]
        public int Id { get; set; }
        [JsonPropertyName("Name")]
        public string Name { get; set; } = string.Empty;
    }
}
