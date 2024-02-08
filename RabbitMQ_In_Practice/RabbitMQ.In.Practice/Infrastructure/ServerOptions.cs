namespace RabbitMQ.In.Practice.Infrastructure {
    public sealed class ServerOptions {
        #region Public Properties

        public string Hostname { get; set; } = "localhost";

        public int Port { get; set; } = 5672;

        public string VirtualHost { get; set; } = "/";

        public string Username { get; set; } = "guest";

        public string Password { get; set; } = "guest";

        #endregion
    }
}
