using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.In.Practice.Infrastructure;

namespace RabbitMQ.In.Practice.Exercises {
    public class WorkerQueueExercise : Exercise {
        private readonly ExchangeDescriptor Exchange = new() {
            Name = "ex.worker",
            AutoDelete = true,
            Durable = false,
            Queues = [
                new QueueDescriptor {
                    Name = "q.worker",
                    AutoDelete = true,
                    Durable = false
                }
            ]
        };

        public WorkerQueueExercise(ChannelFactory channelFactory)
            : base(channelFactory) { }

        public override int Code => 2;

        public override string Description => "Worker queue";

        public override int Run(TextReader input, TextWriter output) {
            using var channel = CreateChannel();

            channel.Assert([Exchange]);

            // We'll create 2 consumers for this exercise
            // Each one of them will answer a message

            var consumerA = new AsyncEventingBasicConsumer(channel);
            consumerA.Received += (_, args) => {
                var message = Message.FromBuffer(args.Body);
                output.WriteLine($"I'm consumer A: {message.Content}");
                output.WriteLine();
                return Task.CompletedTask;
            };
            channel.BasicConsume(Exchange.Queues[0].Name, autoAck: true, consumerA);

            var consumerB = new AsyncEventingBasicConsumer(channel);
            consumerB.Received += (_, args) => {
                var message = Message.FromBuffer(args.Body);
                output.WriteLine($"I'm consumer B: {message.Content}");
                output.WriteLine();
                return Task.CompletedTask;
            };
            channel.BasicConsume(Exchange.Queues[0].Name, autoAck: true, consumerB);

            // Now, let's publish 10 messages
            var publisher = channel.CreateBasicPublishBatch();
            for (var idx = 0; idx < 10; idx++) {
                var message = new Message {
                    Content = $"Message {idx + 1}"
                };

                publisher.Add(
                    exchange: Exchange.Name,
                    routingKey: string.Empty,
                    mandatory: false,
                    properties: channel.CreateBasicProperties(),
                    body: message.ToBuffer()
                );
            }
            publisher.Publish();

            return input.Read();
        }
    }
}
