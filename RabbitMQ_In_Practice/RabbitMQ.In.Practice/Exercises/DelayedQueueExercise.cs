using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.In.Practice.Infrastructure;

namespace RabbitMQ.In.Practice.Exercises {
    public class DelayedQueueExercise : Exercise {
        private const string DLX_EXCHANGE_NAME = "ex.dlx.delayed";
        private const string DLX_QUEUE_NAME = "q.dlx.delayed";

        private const string EXCHANGE_NAME = "ex.delayed";
        private const string QUEUE_NAME = "q.delayed";

        private const string DLX_ROUTING_KEY = "dlx.routing-key";

        private const int TOTAL_MESSAGES = 5;

        private const int WAIT_FOR = 250; // milliseconds

        private readonly ExchangeDescriptor DlxExchange = new() {
            Name = DLX_EXCHANGE_NAME,
            AutoDelete = true,
            Durable = false,
            Queues = [
                new QueueDescriptor {
                    Name = DLX_QUEUE_NAME,
                    AutoDelete = true,
                    Durable = false,
                    Bindings = [
                        new BindingDescriptor {
                            RoutingKey = DLX_ROUTING_KEY,
                        }
                    ]
                }
            ]
        };

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
                        { "x-dead-letter-exchange", DLX_EXCHANGE_NAME },
                        { "x-message-ttl", WAIT_FOR * 10 },
                        { "x-dead-letter-routing-key", DLX_ROUTING_KEY }
                    }
                }
            ]
        };

        public DelayedQueueExercise(ChannelFactory channelFactory)
            : base(channelFactory) { }

        public override int Code => 8;

        public override string Description => "Delayed Queue Sample";

        public override int Run(TextReader input, TextWriter output) {
            // What is the idea behind delayed queue?
            // We'll create a queue with a defined TTL of X ms.
            // And another queue (DLX) that will receive our message
            // when the TTL expires.
            // Our consumer will be listening to the DLX queue and not
            // the actual queue.

            using var channel = CreateChannel();

            channel.Assert([DlxExchange, Exchange]);

            Task.Run(() => Task.WaitAll(
                ProducerAsync(channel, output, CancellationToken.None),
                ConsumerAsync(channel, output, CancellationToken.None)
            ));

            var result = input.Read();

            channel.QueueDelete(QUEUE_NAME);
            channel.QueueDelete(DLX_QUEUE_NAME);
            channel.ExchangeDelete(EXCHANGE_NAME);
            channel.ExchangeDelete(DLX_EXCHANGE_NAME);

            return result;
        }

        private static async Task ProducerAsync(IModel channel, TextWriter output, CancellationToken cancellationToken) {
            for (var idx = 0; idx < TOTAL_MESSAGES; idx++) {
                var content = $"[{idx + 1}] Heeeeeeeellllllooooooo wwwwwoooooorrrrllllddddd....";

                output.WriteLine("[{idx + 1}] Sending message...");

                channel.BasicPublish(
                    EXCHANGE_NAME,
                    string.Empty,
                    channel.CreateBasicProperties(),
                    new Message { Content = content }.ToBuffer()
                );

                await Task.Delay(WAIT_FOR, cancellationToken);
            }
        }

        private static Task ConsumerAsync(IModel channel, TextWriter output, CancellationToken cancellationToken) {
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += (_, args) => {
                var message = Message.FromBuffer(args.Body);
                output.WriteLine($"Message: {message.Content} delivered through exchange {args.Exchange}");
                return Task.CompletedTask;
            };

            channel.BasicConsume(DLX_QUEUE_NAME, autoAck: true, consumer);

            return Task.CompletedTask;
        }
    }
}
