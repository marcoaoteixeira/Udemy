# [RabbitMQ in Practice](https://www.udemy.com/course/rabbitmq-in-practice/)

## Installation and Configuration

Let's use Docker for this, seems a litte bit easier that install the actual RabbitMQ application on your PC.

Use the command below to download and run the Docker image:

`docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 -e RABBITMQ_DEFAULT_USER=guest -e RABBITMQ_DEFAULT_PASS=guest rabbitmq:3.12.12-management-alpine`

This will create a container for RabbitMQ (with Management UI), exposing RabbitMQ service and UI ports. The UI will be accessible through http://localhost:15672

- Username: `guest`
- Password: `guest`

### Enabling Consistent-Hash Plugin

If you're using Docker Desktop (for Windows), open the UI and access your container by clicking the container's name.
Go to the **Exec** tab and execute the command:

`rabbitmq-plugins enable rabbitmq_consistent_hash_exchange`

Or use the interactive console for this.

## Information Resources and Sites

- [RabbitMQ on GitHub](https://github.com/rabbitmq/rabbitmq-server)
- [Get Started with RabbitMQ](https://rabbitmq.com/getstarted.html)
- [Udemy - RabbitMQ in Practice](https://rabbitmq.com/getstarted.html)