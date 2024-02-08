using RabbitMQ.Client;

namespace RabbitMQ.In.Practice.Infrastructure {
    public static class ChannelExtension {
        public static void Assert(this IModel self, ExchangeDescriptor[] exchanges, TextWriter? output = null) {
            var inner = output ?? TextWriter.Null;

            // when we declare a exchange/queue, if the exchange/queue
            // doesn't exists, it will be created for us.

            foreach (var exchange in exchanges) {
                // let's declare our exchange
                inner.WriteLine($"Declaring exchange {exchange.Name}");
                DeclareExchange(self, exchange);
                inner.WriteLine();

                foreach (var queue in exchange.Queues) {
                    // let's declare our queue
                    inner.WriteLine($"Declaring queue {queue.Name} for exchange {exchange.Name}");
                    DeclareQueue(self, queue);

                    // let's declare our bindings
                    foreach (var binding in queue.Bindings) {
                        inner.WriteLine($"Binding queue {exchange.Name}, to exchange {exchange.Name} with routing key {binding.RoutingKey}");
                        DeclareBinding(self, exchange, queue, binding);
                    }
                    inner.WriteLine();
                }
            }
        }

        private static void DeclareBinding(IModel channel, ExchangeDescriptor exchange, QueueDescriptor queue, BindingDescriptor binding)
            => channel.QueueBind(
                queue: queue.Name,
                exchange: exchange.Name,
                routingKey: binding.RoutingKey,
                arguments: binding.Arguments
            );

        private static void DeclareQueue(IModel channel, QueueDescriptor queue)
            => channel.QueueDeclare(
                queue: queue.Name,
                durable: queue.Durable,
                exclusive: queue.Exclusive,
                autoDelete: queue.AutoDelete,
                arguments: queue.Arguments
            );

        private static void DeclareExchange(IModel channel, ExchangeDescriptor exchange)
            => channel.ExchangeDeclare(
                exchange: exchange.Name,
                type: exchange.Type.GetDescription(),
                durable: exchange.Durable,
                autoDelete: exchange.AutoDelete,
                arguments: exchange.Arguments
            );
    }
}
