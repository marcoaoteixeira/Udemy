using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.In.Practice.Infrastructure;

namespace RabbitMQ.In.Practice.Exercises {
    public class DeadLetterExchangeExercise : Exercise {
        private const string DLX_EXCHANGE_NAME = "ex.dlx.deadletter";
        private const string DLX_QUEUE_NAME = "q.dlx.deadletter";
        private const string DLX_ROUTING_KEY = "routing-key.dlx.deadletter";

        private const string EXCHANGE_NAME = "ex.deadletter";
        private const string QUEUE_NAME = "q.deadletter";

        private const int TTL = 500;
        private const int DELAY_BETWEEN_MESSAGES = 250;
        private const int TOTAL_MESSAGES = 10;

        private static readonly bool AutoDelete;
        private static readonly bool Durable = true;

        private readonly ExchangeDescriptor DlxExchange = new() {
            Name = DLX_EXCHANGE_NAME,
            Type = Infrastructure.ExchangeType.Direct,
            AutoDelete = AutoDelete,
            Durable = Durable,
            Queues = [
                new QueueDescriptor {
                    Name = DLX_QUEUE_NAME,
                    AutoDelete = AutoDelete,
                    Durable = Durable,
                    Bindings = [
                        new BindingDescriptor {
                            RoutingKey = DLX_ROUTING_KEY
                        }
                    ]
                }
            ]
        };

        private readonly ExchangeDescriptor Exchange = new() {
            Name = EXCHANGE_NAME,
            Type = Infrastructure.ExchangeType.Direct,
            AutoDelete = AutoDelete,
            Durable = Durable,
            Queues = [
                new QueueDescriptor {
                    Name = QUEUE_NAME,
                    AutoDelete = AutoDelete,
                    Durable = Durable,
                    Arguments = {
                        { "x-dead-letter-exchange", DLX_EXCHANGE_NAME },
                        { "x-message-ttl", TTL },
                        { "x-dead-letter-routing-key", DLX_ROUTING_KEY }
                    }
                }
            ]
        };

        public DeadLetterExchangeExercise(ChannelFactory channelFactory)
            : base(channelFactory) { }

        public override int Code => 7;

        public override string Description => "Dead Letter Exchange Sample";

        public override int Run(TextReader input, TextWriter output) {
            using var channel = CreateChannel();

            channel.Assert([DlxExchange, Exchange]);

            var cts = new CancellationTokenSource();

            Task.Run(() => {
                try {
                    Task.WaitAll(
                        RunProducerAsync(output, cts.Token),
                        RunConsumerAsync(output, cts.Token),
                        RunDeadLetterAsync(output, cts.Token)
                    );
                }
                catch (AggregateException) { /* swallow */ }
            }, cts.Token);

            var result = input.Read();

            cts.Cancel();

            channel.QueueDelete(QUEUE_NAME);
            channel.QueueDelete(DLX_QUEUE_NAME);

            channel.ExchangeDelete(EXCHANGE_NAME);
            channel.ExchangeDelete(DLX_EXCHANGE_NAME);

            return result;
        }

        private async Task RunProducerAsync(TextWriter output, CancellationToken cancellationToken) {
            using var channel = CreateChannel();

            var basicProps = channel.CreateBasicProperties();

            for (var idx = 0; idx < TOTAL_MESSAGES; idx++) {
                var message = new Message {
                    ID = idx + 1,
                    Content = "Hello world!"
                };
                var buffer = message.ToBuffer();

                channel.BasicPublish(
                    EXCHANGE_NAME,
                    routingKey: string.Empty,
                    basicProps,
                    buffer
                );
                output.WriteLine($"Message #{idx + 1} sent...");

                await Task.Delay(DELAY_BETWEEN_MESSAGES, cancellationToken);
            }
        }

        private async Task RunConsumerAsync(TextWriter output, CancellationToken cancellationToken) {
            // Let's wait a little bit so the Producer can send the
            // first message.
            await Task.Delay(DELAY_BETWEEN_MESSAGES, cancellationToken);
            
            using var channel = CreateChannel();

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.Received += (sender, args) => {
                if (sender is IBasicConsumer consumer) {
                    var message = Message.FromBuffer(args.Body);

                    output.WriteLine($"Ignoring message #{message.ID}");

                    consumer.Model.BasicNack(
                        args.DeliveryTag,
                        multiple: false,
                        requeue: false
                    );
                }

                return Task.CompletedTask;
            };

            channel.BasicConsume(QUEUE_NAME, autoAck: false, consumer);

            await Task.Delay((TOTAL_MESSAGES - (TOTAL_MESSAGES / 2)) * DELAY_BETWEEN_MESSAGES, cancellationToken);

            // When exit, this will trigger the TTL for the queue.
        }

        private async Task RunDeadLetterAsync(TextWriter output, CancellationToken cancellationToken) {
            // Let's wait a little bit so the Producer and Producer can
            // start.
            await Task.Delay(DELAY_BETWEEN_MESSAGES * 2, cancellationToken);

            using var channel = CreateChannel();

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.Received += (_, args) => {
                var message = Message.FromBuffer(args.Body);
                var reasonBuffer = args.BasicProperties.Headers["x-first-death-reason"] as byte[];
                var reason = reasonBuffer is not null
                    ? Encoding.UTF8.GetString(reasonBuffer)
                    : string.Empty;

                output.WriteLine($"[{reason}] DLX: Received message #{message.ID}");

                return Task.CompletedTask;
            };

            channel.BasicConsume(DLX_QUEUE_NAME, autoAck: true, consumer);

            await Task.Delay(millisecondsDelay: int.MaxValue, cancellationToken);
        }
    }
}
