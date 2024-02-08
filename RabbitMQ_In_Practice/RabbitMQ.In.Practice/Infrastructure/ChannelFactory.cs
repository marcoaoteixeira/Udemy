using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace RabbitMQ.In.Practice.Infrastructure {
    public class ChannelFactory : IDisposable {
        private readonly IConfiguration _configuration;

        private ServerOptions? _serverOptions;
        private ConnectionFactory? _connectionFactory;
        private IConnection? _connection;
        private bool _disposed;

        public ChannelFactory(IConfiguration configuration) {
            _configuration = configuration;
        }

        public IModel CreateChannel() {
            BlockAccessAfterDispose();

            return GetConnection().CreateModel();
        }

        protected virtual void Dispose(bool disposing) {
            if (_disposed) { return; }
            if (disposing) {
                _connection?.Dispose();
            }

            _connectionFactory = null;
            _connection = null;
            _disposed = true;
        }

        private void BlockAccessAfterDispose()
            => ObjectDisposedException.ThrowIf(_disposed, typeof(ChannelFactory));

        private ServerOptions GetServerOptions()
            => _serverOptions ??= _configuration
                .GetSection("RabbitMQ")
                .Get<ServerOptions>() ?? new();

        private ConnectionFactory GetConnectionFactory()
            => _connectionFactory ??= new ConnectionFactory {
                HostName = GetServerOptions().Hostname,
                Port = GetServerOptions().Port,
                VirtualHost = GetServerOptions().VirtualHost,

                // We're using AsyncEventingBasicConsumer
                // so, we must set this to true
                // https://stackoverflow.com/questions/47847590/explain-asynceventingbasicconsumer-behaviour-without-dispatchconsumersasync-tr
                DispatchConsumersAsync = true,

                UserName = GetServerOptions().Username,
                Password = GetServerOptions().Password
            };

        private IConnection GetConnection()
            => _connection ??= GetConnectionFactory().CreateConnection();

        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
