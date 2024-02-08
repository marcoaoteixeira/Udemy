using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.In.Practice.Infrastructure;

namespace RabbitMQ.In.Practice.Exercises {
    public class ConsumerTransactionExercise : Exercise {
        private const string EXCHANGE_NAME = "ex.consumer.transaction";
        private const string QUEUE_NAME = "q.consumer.transaction";

        private const int TOTAL_MESSAGES = 5;

        private const int WAIT_FOR = 250; // milliseconds

        private readonly ExchangeDescriptor Exchange = new() {
            Name = EXCHANGE_NAME,
            AutoDelete = true,
            Durable = false,
            Queues = [
                new QueueDescriptor {
                    Name = QUEUE_NAME,
                    AutoDelete = true,
                    Durable = false
                }
            ]
        };

        public ConsumerTransactionExercise(ChannelFactory channelFactory)
            : base(channelFactory) { }

        public override int Code => 9;

        public override string Description => "Consumer Transaction Sample";

        public override int Run(TextReader input, TextWriter output) {
            // In this example, we'll see how to create a transaction
            // to ensure message delivering and consuming by the consumer
            // using channel.TxSelect() and channel.TxCommit()
            // or channel.TxRollback() and channel.BasicCancel().

            using var channel = CreateChannel();

            channel.Assert([Exchange]);

            var cts = new CancellationTokenSource();

            Task.Run(() => ProducerAsync(output, cts.Token), cts.Token);
            Task.Run(() => ConsumerAsync(output, cts.Token), cts.Token);

            var result = input.Read();

            cts.Cancel();

            channel.QueueDelete(QUEUE_NAME);
            channel.ExchangeDelete(EXCHANGE_NAME);

            return result;
        }

        private async Task ProducerAsync(TextWriter output, CancellationToken cancellationToken) {
            using var channel = CreateChannel();

            for (var idx = 0; idx < TOTAL_MESSAGES; idx++) {
                var content = idx != 4
                    ? $"[{idx + 1}] Hello world"
                    : $"[{idx + 1}] END";

                output.WriteLine($"[x] Sending message {content}");

                channel.BasicPublish(
                    EXCHANGE_NAME,
                    string.Empty,
                    channel.CreateBasicProperties(),
                    new Message { Content = content }.ToBuffer()
                );

                await Task.Delay(WAIT_FOR, cancellationToken);
            }

            await Task.Delay(int.MaxValue, cancellationToken);
        }

        private async Task ConsumerAsync(TextWriter output, CancellationToken cancellationToken) {
            using var channel = CreateChannel();

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += (sender, args) => {
                if (sender is AsyncEventingBasicConsumer evt) {
                    evt.Model.TxSelect();

                    var message = Message.FromBuffer(args.Body);
                    output.WriteLine($"[x] Received message {message.Content}");
                    evt.Model.BasicAck(args.DeliveryTag, multiple: false);

                    if (message.Content.Contains("END")) {
                        evt.Model.TxCommit();

                        // Try comment the line above and uncomment lines below.
                        // After that, check your RabbitMQ management UI.

                        // Or to rollback the transaction
                        //evt.Model.TxRollback();

                        // this will put all messages back into the
                        // queue to be consumed by another consumer
                        //evt.Model.BasicCancel(args.ConsumerTag);
                    }
                }

                return Task.CompletedTask;
            };

            channel.BasicConsume(QUEUE_NAME, autoAck: false, consumer);

            await Task.Delay(int.MaxValue, cancellationToken);
        }
    }
}
