using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.In.Practice.Infrastructure;

namespace RabbitMQ.In.Practice.Exercises {
    public class PubSubTopicExercise : Exercise {
        private const string WHATEVER_ORANGE_WHATEVER_TOPIC = "*.orange.*";
        private const string WHATEVER_WHATEVER_RABBIT_TOPIC = "*.*.rabbit";
        private const string LAZY_ANYTHING_TOPIC = "lazy.#";

        private readonly ExchangeDescriptor Exchange = new() {
            Name = "ex.topic",
            Type = Infrastructure.ExchangeType.Topic,
            AutoDelete = true,
            Durable = false,
            Queues = [
                new QueueDescriptor {
                    Name = "q.topic.a",
                    AutoDelete = true,
                    Durable = false,
                    Bindings = [
                        new BindingDescriptor {
                            // will receive messagens from anything like
                            // CELERITY.orange.SPECIES
                            RoutingKey = WHATEVER_ORANGE_WHATEVER_TOPIC
                        }
                    ]
                },
                new QueueDescriptor {
                    Name = "q.topic.b",
                    AutoDelete = true,
                    Durable = false,
                    Bindings = [
                        // will receive messagens from anything like
                        // CELERITY.COLOUR.rabbit
                        new BindingDescriptor {
                            RoutingKey = WHATEVER_WHATEVER_RABBIT_TOPIC
                        },
                        // lazy.COLOUR.SPECIES
                        // lazy.BLABLABLA.BLABLABLA.BLABLABLA
                        // lazy.
                        new BindingDescriptor {
                            RoutingKey = LAZY_ANYTHING_TOPIC
                        }
                    ]
                }
            ]
        };

        public PubSubTopicExercise(ChannelFactory channelFactory)
            : base(channelFactory) { }

        public override int Code => 5;

        public override string Description => "Pub/Sub topic queue";

        public override int Run(TextReader input, TextWriter output) {
            using var channel = CreateChannel();

            channel.Assert([Exchange], output);

            // let's assume that we have a pattern for our routing key
            // we'll talk about species of animals, their colour and celerity.
            // so, something like this will serve us <celerity>.<colour>.<species>

            // think about it, we could have routing keys as
            // quick.brown.fox
            // lazy.purple.elephant
            // etc...

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
            var messages = new Dictionary<string, string> {
                { "quick.brown.fox", "Quick Brown Fox" },
                { "fast.orange.tiger", "Fast Orange Tiger" },
                { "normal.orange.dog", "Caramelo Dog" },
                { "lazy.green.turtle", "Lazy Green Turtle" },
                { "lazy.orange.horse", "Lazy Orange Horse" },
                { "lazy.bla.bla.bla", "Lazy Something..." }
            };
            var publisher = channel.CreateBasicPublishBatch();
            foreach (var message in messages) {
                publisher.Add(
                    exchange: Exchange.Name,
                    routingKey: message.Key,
                    mandatory: false,
                    properties: channel.CreateBasicProperties(),
                    body: new Message {
                        Content = message.Value
                    }.ToBuffer()
                );
            }
            publisher.Publish();

            return input.Read();
        }
    }
}
