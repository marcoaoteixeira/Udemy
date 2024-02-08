using RabbitMQ.In.Practice.Infrastructure;

namespace RabbitMQ.In.Practice.Exercises {
    public class ConsistentHashExchangeExercise : Exercise {
        private readonly ExchangeDescriptor Exchange = new() {
            Name = "ex.consistent-hash",
            Type = ExchangeType.ConsistentHash,
            AutoDelete = true,
            Durable = false,
            Queues = [
                new QueueDescriptor {
                    Name = "q.consistent-hash-1",
                    AutoDelete = true,
                    Durable = false,
                    Bindings = [
                        new BindingDescriptor {
                            // Here, the routing key will be the WEIGHT
                            // for our consistent hash exchange. So it 
                            // must be an integer.
                            RoutingKey = "1"
                        }
                    ]
                },
            new QueueDescriptor {
                Name = "q.consistent-hash-2",
                AutoDelete = true,
                Durable = false,
                Bindings = [
                        new BindingDescriptor {
                            // Here, our WEIGHT is 3, meaning that
                            // we expect this queue to receive, at least,
                            // the 3 times more messages than the first queue.
                            RoutingKey = "3"
                        }
                    ]
                }
            ]
        };

        public ConsistentHashExchangeExercise(ChannelFactory channelFactory)
            : base(channelFactory) { }

        public override int Code => 6;

        public override string Description => "Consistent Hash Exchange";

        public override int Run(TextReader input, TextWriter output) {
            using var channel = CreateChannel();

            channel.Assert([Exchange]);

            var basicProps = channel.CreateBasicProperties();

            // Now, let's publish X messages
            var count = 300;
            for (var idx = 0; idx < count; idx++) {
                output.WriteLine($"Sending message #{idx + 1} of {count}...");
                var message = new Message {
                    Content = $"Message {idx + 1}"
                };
                channel.BasicPublish(
                    exchange: Exchange.Name,
                    
                    // When publishing, the routing key should be
                    // something that identifies the messages.
                    // Doing it so, the hash plugin will send the message
                    // to the queues as expected. If you use the same
                    // routing key for every message, probably just one
                    // queue will receive it, defeating the purpose of
                    // the consistent hash.
                    routingKey: $"Hash me! #{idx + 1}",

                    mandatory: false,
                    basicProperties: basicProps,
                    body: message.ToBuffer()
                );
            }

            output.WriteLine("Go check your RabbitMQ management console and see how many messages are on both queues...");
            output.WriteLine("Click any key on your keyboard to continue. This will also delete the queues and exchange...");

            var result = input.Read();

            channel.QueuePurge(Exchange.Queues[0].Name);
            channel.QueueDelete(Exchange.Queues[0].Name, true, true);
            channel.QueuePurge(Exchange.Queues[1].Name);
            channel.QueueDelete(Exchange.Queues[1].Name, true, true);

            return result;
        }
    }
}
