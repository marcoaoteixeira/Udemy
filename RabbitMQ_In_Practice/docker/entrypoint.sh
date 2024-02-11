#!/bin/sh

# exit immediately if a pipeline returns a non-zero status
set -e

# change .erlang.cookie permission
chmod 400 /var/lib/rabbitmq/.erlang.cookie

# get hostname from enviromant variable
HOSTNAME=`env hostname`

echo "Starting RabbitMQ Server For host:" $HOSTNAME

# if $RABBITMQ_JOIN_CLUSTER param was provided
if [ -z "$RABBITMQ_JOIN_CLUSTER" ]; then
    /usr/local/bin/docker-entrypoint.sh rabbitmq-server &
    sleep 5
    rabbitmqctl wait /var/lib/rabbitmq/mnesia/$RABBITMQ_NODENAME\@$HOSTNAME.pid
else
    /usr/local/bin/docker-entrypoint.sh rabbitmq-server -detached
    sleep 5
    rabbitmqctl wait /var/lib/rabbitmq/mnesia/$RABBITMQ_NODENAME\@$HOSTNAME.pid
    rabbitmqctl stop_app
    rabbitmqctl join_cluster $RABBITMQ_JOIN_CLUSTER
    rabbitmqctl start_app
fi

# Keep foreground process active ...
tail -f /dev/null