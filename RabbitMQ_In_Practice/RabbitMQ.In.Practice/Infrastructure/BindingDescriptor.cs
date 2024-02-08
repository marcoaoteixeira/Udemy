﻿namespace RabbitMQ.In.Practice.Infrastructure {
    public sealed class BindingDescriptor {
        #region Private Fields

        private string? _routingKey;
        private Dictionary<string, object>? _arguments;

        #endregion

        #region Public Properties

        public string RoutingKey {
            get => _routingKey ??= string.Empty;
            set => _routingKey = value ?? string.Empty;
        }

        public Dictionary<string, object> Arguments {
            get => _arguments ??= [];
            set => _arguments = value ?? [];
        }

        #endregion
    }
}
