using RabbitMQ.Client;

namespace RabbitMQ.In.Practice.Infrastructure {
    public abstract class Exercise {
        private readonly ChannelFactory _channelFactory;

        public abstract int Code { get; }

        public abstract string Description { get; }

        protected IModel CreateChannel()
            => _channelFactory.CreateChannel();

        protected Exercise(ChannelFactory channelFactory) {
            _channelFactory = channelFactory;
        }

        public abstract int Run(TextReader input, TextWriter output);
    }
}
