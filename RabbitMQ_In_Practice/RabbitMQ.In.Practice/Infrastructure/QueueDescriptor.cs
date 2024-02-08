namespace RabbitMQ.In.Practice.Infrastructure {
    public sealed class QueueDescriptor {
        #region Private Fields

        private Dictionary<string, object>? _arguments;
        private BindingDescriptor[]? _bindings;
        private string? _name;

        #endregion

        #region Public Properties

        public string Name {
            get => _name ??= "q.default";
            set => _name = string.IsNullOrWhiteSpace(value)
                ? throw new ArgumentException(null, nameof(value))
                : value;
        }

        public bool Durable { get; set; } = true;

        public bool Exclusive { get; set; }

        public bool AutoDelete { get; set; }

        public Dictionary<string, object> Arguments {
            get => _arguments ??= [];
            set => _arguments = value ?? [];
        }

        public BindingDescriptor[] Bindings {
            get => _bindings ??= [new()];
            set => _bindings = value is null || value.Length == 0
                ? [new()]
                : value;
        }

        #endregion
    }
}
