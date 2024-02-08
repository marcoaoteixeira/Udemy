namespace RabbitMQ.In.Practice.Infrastructure {
    public sealed class ExchangeDescriptor {
        #region Private Fields

        private Dictionary<string, object>? _arguments;
        private QueueDescriptor[]? _queues;
        private string? _name;

        #endregion

        #region Public Properties

        public string Name {
            get => _name ??= string.Empty;
            set => _name = value ?? string.Empty;
        }

        public ExchangeType Type { get; set; }

        public bool Durable { get; set; } = true;

        public bool AutoDelete { get; set; }

        public Dictionary<string, object> Arguments {
            get => _arguments ??= [];
            set => _arguments = value ?? [];
        }

        public QueueDescriptor[] Queues {
            get => _queues ??= [];
            set => _queues = value ?? [];
        }

        #endregion
    }
}
