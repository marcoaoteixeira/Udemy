﻿using System.ComponentModel;

namespace RabbitMQ.In.Practice.Infrastructure {
    public enum ExchangeType {
        [Description("direct")]
        Direct = 0,
        [Description("topic")]
        Topic = 1,
        [Description("queue")]
        Queue = 2,
        [Description("fanout")]
        Fanout = 4,
        [Description("headers")]
        Headers = 8,
        [Description("x-consistent-hash")]
        ConsistentHash = 16
    }
}
