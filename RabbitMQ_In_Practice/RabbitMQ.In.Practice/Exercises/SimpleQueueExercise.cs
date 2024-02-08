using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.In.Practice.Infrastructure;

namespace RabbitMQ.In.Practice.Exercises {
    public class SimpleQueueExercise : Exercise {
        private readonly ExchangeDescriptor Exchange = new() {
            Name = "ex.default",
            AutoDelete = true,
            Durable = false,
            Queues = [
                new QueueDescriptor {
                    Name = "q.default",
                    AutoDelete = true,
                    Durable = false
                }
            ]
        };

        public SimpleQueueExercise(ChannelFactory channelFactory)
            : base(channelFactory) { }

        public override int Code => 1;

        public override string Description => "Simple queue";

        public override int Run(TextReader input, TextWriter output) {
            using var channel = CreateChannel();

            channel.Assert([Exchange]);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += (_, args) => {
                var message = Message.FromBuffer(args.Body);
                output.WriteLine(message.Content);
                return Task.CompletedTask;
            };

            channel.BasicConsume(Exchange.Queues[0].Name, autoAck: true, consumer);

            var message = new Message {
                Content = "Hello RabbitMQ!"
            };
            channel.BasicPublish(Exchange.Name, string.Empty, body: message.ToBuffer());

            return input.Read();
        }
    }
}
