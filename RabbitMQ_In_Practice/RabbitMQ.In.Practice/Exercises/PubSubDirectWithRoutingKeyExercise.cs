using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.In.Practice.Infrastructure;

namespace RabbitMQ.In.Practice.Exercises {
    public class PubSubDirectWithRoutingKeyExercise : Exercise {
        private const string SPORT_ROUTING_KEY = "sport";
        private const string WEATHER_ROUTING_KEY = "weather";

        private readonly ExchangeDescriptor Exchange = new() {
            Name = "ex.direct.routing.key",
            Type = Infrastructure.ExchangeType.Direct,
            AutoDelete = true,
            Durable = false,
            Queues = [
                new QueueDescriptor {
                    Name = "q.direct.routing.key.a",
                    AutoDelete = true,
                    Durable = false,
                    Bindings = [
                        new BindingDescriptor {
                            RoutingKey = SPORT_ROUTING_KEY
                        }
                    ]
                },
                new QueueDescriptor {
                    Name = "q.direct.routing.key.b",
                    AutoDelete = true,
                    Durable = false,
                    Bindings = [
                        new BindingDescriptor {
                            RoutingKey = SPORT_ROUTING_KEY
                        },
                        new BindingDescriptor {
                            RoutingKey = WEATHER_ROUTING_KEY
                        }
                    ]
                }
            ]
        };

        public PubSubDirectWithRoutingKeyExercise(ChannelFactory channelFactory)
            : base(channelFactory) { }

        public override int Code => 4;

        public override string Description => "Pub/Sub direct with routing key queue";

        public override int Run(TextReader input, TextWriter output) {
            using var channel = CreateChannel();
            
            channel.Assert([Exchange]);

            // We'll create 2 consumers for this exercise
            // Both will receive the message related to
            // their routing keys

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
            Message[] messages = [
                new Message {
                    Content = "!!! BROADCAST !!!"
                },
                new Message {
                    Content = "!!! BROADCAST !!!"
                }
            ];
            var publisher = channel.CreateBasicPublishBatch();

            publisher.Add(
                exchange: Exchange.Name,
                routingKey: SPORT_ROUTING_KEY,
                mandatory: false,
                properties: channel.CreateBasicProperties(),
                body: new Message {
                    Content = "Sport News Of The Day"
                }.ToBuffer()
            );

            publisher.Add(
                exchange: Exchange.Name,
                routingKey: WEATHER_ROUTING_KEY,
                mandatory: false,
                properties: channel.CreateBasicProperties(),
                body: new Message {
                    Content = "Your Daily Weather News"
                }.ToBuffer()
            );

            publisher.Publish();

            return input.Read();
        }
    }
}
