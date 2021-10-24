# message-handling [![Build Status](https://github.com/InformatieVlaanderen/message-handling/workflows/CI/badge.svg)](https://github.com/Informatievlaanderen/message-handling/actions)

A lib for handling messages with RabbitMQ for Digitaal Vlaanderen

Currenty supported message types: `Topic`, `Direct`

## Sample with RabbitMQ

### Connection
  a connection should be recycled. Preferably in a Singleton or Connection Pool. 

``` Csharp
          # Example
           var factory = new ConnectionFactory()
            {
                Uri = new Uri("amqps://URL:5671"),
                UserName = "admin",
                Password = "123456789abc",
                Port = 5671
            };
            using var connection = factory.CreateConnection();
```
### Producer
### Consumer
