using System.Text;
using System.Text.Json;

namespace RabbitMQ.In.Practice.Infrastructure {
    public record Message {
        public int ID { get; init; }
        public string Content { get; init; } = string.Empty;

        public ReadOnlyMemory<byte> ToBuffer() {
            var json = JsonSerializer.Serialize(this);
            return Encoding.UTF8.GetBytes(json);
        }

        public static Message FromBuffer(ReadOnlyMemory<byte> buffer) {
            var json = Encoding.UTF8.GetString(buffer.ToArray());
            var result = JsonSerializer.Deserialize<Message>(json);

            return result is null
                ? new() { Content = "!!!NULL MESSAGE!!!" }
                : result;
        }
    }
}
