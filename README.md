# New Document# message-handling [![Build Status](https://github.com/InformatieVlaanderen/message-handling/workflows/CI/badge.svg)](https://github.com/Informatievlaanderen/message-handling/actions)

Lightweight message handling infrastructure for Digitaal Vlaanderen

Currenty supported RabbitMQ

* Message types: `Topic`, `Direct`

## Sample with RabbitMQ

### Initial setup

1. Define your `MessageHandling` configurations in the `Appsettings.json`.
```js
// Appsettings.json  
{
  "MessageHandling": {
    "RabbitMq": {
      "Uri": "XXX",
      "Username": "admin",
      "Password": "123456789abc",
      "Port": 5671
    },
    "Environment": "dev",
    "ThisModule": "municipality-registry" // The module name of your project
    "OtherModules": [
      "address-registry",
      "streetname-registry",
      "building-registry",
      "parcel-registry",
      "postal-registry"
    ]
  },
  ...
}
```
2. Add the message handling service to your servicecollection. This will allow you to inject the `MessageHandlerContext` class
```csharp
// Starup.cs
public class Startup
{
   private IConfigurationRoot _configuration;

   public Startup(IConfigurationRoot configuration, params string[] args)
   {
     _configuration = configuration;
   }
   
   public void ConfigureServices(IServiceCollection serviceCollection)
   {
     var myConfig = _configuration.GetSection("MessageHandling").Get<MessageHandlerConfiguration>();
     serviceCollection.AddMessageHandling(myConfig);
     ...
   }
}
```

### Setup the Producer

#### MessageType

In RabbitMq you can many ways to send messages. This lib only supports 2 types: `Direct` & `Topic`.

In case of a one to one relationship, one publisher and one subscriber, a direct message type is chosen.
In case of a one to many relationship, one publisher and many subscriber, a topic message type is chosen.
subscribers can be added gradually but won't receive past messages.


#### Create

To publish a message we simply create a class and inherit from the <br/>
`TopicProducer<T>` for a `Topic` or `DirectProducer<T>` for a `Direct`.
The `T` is the message class.<br/>

Add the MessageHandlerContext in your contructor and pass it to the base class.
The library will take care of routing.

The Producer class allows you to override 2 methods.
||||
|--|----|--|
|`OnPublishMessagesHandler` | Gets triggered when a message is send succesfully. | `virtual method`|
|`OnPublishMessagesExceptionHand` | Gets triggered when a message still fails to send after all retries. | `abstract method`|


```csharp
public class MessagePublisher : TopicProducer<Message>
{
  public MessagePublisher(MessageHandlerContext context) : base(context) { }

  protected override void OnPublishMessagesHandler(Message[] messages)
  {
      Console.WriteLine("Message send :)");
  }

  protected override void OnPublishMessagesExceptionHand(Exception exception, Message[] messages)
  {
      Console.WriteLine(exception.Message);
  }
}
```

#### Register
Once the Producer is created we have to register it in the serviceCollection. This has to be <strong>Scoped</strong>.

```csharp
// Starup.cs
public class Startup
{
   private IConfigurationRoot _configuration;

   public Startup(IConfigurationRoot configuration, params string[] args)
   {
     _configuration = configuration;
   }
   
   public void ConfigureServices(IServiceCollection serviceCollection)
   {
     var myConfig = _configuration.GetSection("MessageHandling").Get<MessageHandlerConfiguration>();
     serviceCollection.AddMessageHandling(myConfig);
     serviceCollection.AddScoped<MessagePublisher>();
     ...
   }
}
```
#### Demo

To start publishing message(s), inject the message producer class and invoke its `Publish` method.

```csharp
public class App
{
    private readonly MessagePublisher _publisher;

    public App(MessagePublisher publisher)
    {
        _publisher = publisher;
    }

    public async Task Run()
    {
        Console.Write("Publisher");
        while (true)
        {
            #region Generate Messages

            Console.Write("\nDo you want to send a message? (Y/N): ");
            var keypress = Console.ReadKey();
            if (keypress.Key != ConsoleKey.Y && keypress.Key != ConsoleKey.N) continue;
            if (keypress.Key == ConsoleKey.N) break;
            Console.WriteLine("\nHow many messages do you want to send?");
            var input = Console.ReadLine()!;
            int count = int.Parse(input);
            List<Message> messages = new();
            for (int i = 0; i < count; i++)
            {
                messages.Add(new Message()
                {
                    Name = $"Message No.: {i}",
                    Content = "Lorum Ipsum",
                    Version = i
                });
            }

            #endregion

            _publisher.Publish(messages.ToArray());
        }
    }
}
```

### Setup the Consumer
#### Create
To consume a message we follow the same steps as we did for the producer. We create a consumer class that inherits from `TopicConsumer<T>` or `DirectConsumer<T>` where `T` is message class.
In the constructor we pass the module name of the producer to the base. 

The Consumer class allows you to override 3 methods.
||||
|--|----|--|
|`Parse` | Allows you to parse the string message to the `T`. | `abstract method`|
|`MessageReceive` | Gets triggered when a message is received and parsed succesfully. | `abstract method`|
|`MessageReceiveException` | Gets triggered when a message fails to process after all retries. | `abstract method`|


```csharp
public class MessageConsumer : TopicConsumer<Message>
{
    public MessageConsumer(MessageHandlerContext context) : base(
                context,
                new Module("municipality-registry"))
        {
        }

    protected override Message Parse(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            return null!;
        return JsonSerializer.Deserialize<Message>(message)!;
    }

    protected override void MessageReceive(Message message, ulong deliveryTag)
    {
        Console.WriteLine(message.ToString());

        Ack(deliveryTag);
    }

    protected override void MessageReceiveException(Exception exception, ulong deliveryTag)
    {
        Reject(deliveryTag);
    }
}
```

##### Ack
Once the message is received, parsed and processed. We need to acknowledge it. This way it gets thrown out of the queue.
We call the `Ack` method and pass the deliveryTag. This tag is given by rabbitmq self.

##### Nack
When there is a bug consumer application that doesn't allow us to process the message. We can call the `Nack`method and pass the deliveryTag. Here we get 2 options. 
Either we're allowing it to requeue or send it straight to the, DLX, deadletter-exchange. 

A requeue will keep the message in the current queue and will send that message again and again to consumer application until it is Acknowleged. This results in a clog in the queue when the issue isn't solved.
NOOPS
Sending it to the `DLX` will free the queue from that message. So other messages can still be processed. The bad message will stored in seperate queue for developers to take a look at.

##### Reject
The `reject` method sends the message straight to the `DLX`.
<strong>NOTE: Message with <u>NoOps</u> should also be send to reject.</strong>

#### Register
Once the Consumer is created we have to register it in the serviceCollection. This has to be <strong>Scoped</strong>.

```csharp
// Starup.cs
public class Startup
{
   private IConfigurationRoot _configuration;

   public Startup(IConfigurationRoot configuration, params string[] args)
   {
     _configuration = configuration;
   }
   
   public void ConfigureServices(IServiceCollection serviceCollection)
   {
     var myConfig = _configuration.GetSection("MessageHandling").Get<MessageHandlerConfiguration>();
     serviceCollection.AddMessageHandling(myConfig);
     serviceCollection.AddScoped<MessageConsumer>();
     ...
   }
}
```
#### Demo

To start consuming messages, inject the message consumer class and invoke its `Watch` method.

```csharp
public class App
{
    private readonly ILogger<App> _logger;

    public App(MessageConsumer consumer)
    {
        _consumer = consumer;
    }

    public async Task Run()
    {
        _consumer.Watch();
        Console.WriteLine("Waiting for messages.");
    }
}
```
