using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.In.Practice.Infrastructure;

namespace RabbitMQ.In.Practice.Exercises {
    public class PriorityQueueExercise : Exercise {
        private const string EXCHANGE_NAME = "ex.priority";
        private const string QUEUE_NAME = "q.priority";

        private const int TOTAL_MESSAGES = 5;

        private const int WAIT_FOR = 500; // milliseconds

        private readonly ExchangeDescriptor Exchange = new() {
            Name = EXCHANGE_NAME,
            AutoDelete = true,
            Durable = false,
            Queues = [
                new QueueDescriptor {
                    Name = QUEUE_NAME,
                    AutoDelete = true,
                    Durable = false,
                    Arguments = {
                        // This will inform RabbitMQ that this queue has
                        // the message priorization enabled. Stating that
                        // the max priority is 10 (max value is 255)
                        { "x-max-priority", 10 }
                    }
                }
            ]
        };

        public PriorityQueueExercise(ChannelFactory channelFactory)
            : base(channelFactory) { }

        public override int Code => 11;

        public override string Description => "Priority Message Queue Sample";

        public override int Run(TextReader input, TextWriter output) {
            // This example will show us how to send prioritized messages

            using var channel = CreateChannel();

            channel.Assert([Exchange]);

            Produce(channel, output);

            Thread.Sleep(WAIT_FOR);

            Consume(channel, output);

            var result = input.Read();

            channel.QueueDelete(QUEUE_NAME);
            channel.ExchangeDelete(EXCHANGE_NAME);

            return result;
        }

        private static void Produce(IModel channel, TextWriter output) {
            for (var idx = 0; idx < TOTAL_MESSAGES; idx++) {
                var properties = channel.CreateBasicProperties();
                properties.Priority = (byte)Random.Shared.Next(1, 10);

                var content = $"[{idx + 1}] Hello world - Priority {properties.Priority}";

                output.WriteLine($"[x] Sending message {content}");

                channel.BasicPublish(
                    EXCHANGE_NAME,
                    string.Empty,
                    properties,
                    new Message { Content = content }.ToBuffer()
                );
            }
        }

        private static void Consume(IModel channel, TextWriter output) {
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += (_, args) => {
                var message = Message.FromBuffer(args.Body);
                output.WriteLine($"[x] Received message {message.Content}");
                return Task.CompletedTask;
            };

            channel.BasicConsume(QUEUE_NAME, autoAck: true, consumer);
        }
    }
}
