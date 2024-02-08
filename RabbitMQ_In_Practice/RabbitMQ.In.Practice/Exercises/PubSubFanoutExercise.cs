using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.In.Practice.Infrastructure;

namespace RabbitMQ.In.Practice.Exercises {
    public class PubSubFanoutExercise : Exercise {
        private readonly ExchangeDescriptor Exchange = new() {
            Name = "ex.fanout",
            Type = Infrastructure.ExchangeType.Fanout,
            AutoDelete = true,
            Durable = false,
            Queues = [
                new QueueDescriptor {
                    Name = "q.fanout.a",
                    AutoDelete = true,
                    Durable = false
                },
                new QueueDescriptor {
                    Name = "q.fanout.b",
                    AutoDelete = true,
                    Durable = false
                }
            ]
        };

        public PubSubFanoutExercise(ChannelFactory channelFactory)
            : base(channelFactory) { }

        public override int Code => 3;

        public override string Description => "Pub/Sub fanout queue";

        public override int Run(TextReader input, TextWriter output) {
            using var channel = CreateChannel();

            channel.Assert([Exchange]);

            // We'll create 2 consumers for this exercise
            // Both will receive the same message

            var consumerA = new AsyncEventingBasicConsumer(channel);
            consumerA.Received += (sender, args) => {
                var message = Message.FromBuffer(args.Body);
                output.WriteLine($"I'm consumer A: {message.Content}");
                output.WriteLine();
                return Task.CompletedTask;
            };
            channel.BasicConsume(Exchange.Queues[0].Name, autoAck: true, consumerA);

            var consumerB = new AsyncEventingBasicConsumer(channel);
            consumerB.Received += (sender, args) => {
                var message = Message.FromBuffer(args.Body);
                output.WriteLine($"I'm consumer B: {message.Content}");
                output.WriteLine();
                return Task.CompletedTask;
            };
            channel.BasicConsume(Exchange.Queues[1].Name, autoAck: true, consumerB);

            // Now, let's publish a broadcast message
            var message = new Message {
                Content = "!!! BROADCAST !!!"
            };
            channel.BasicPublish(
                exchange: Exchange.Name,
                routingKey: string.Empty,
                mandatory: false,
                basicProperties: channel.CreateBasicProperties(),
                body: message.ToBuffer()
            );

            return input.Read();
        }
    }
}
