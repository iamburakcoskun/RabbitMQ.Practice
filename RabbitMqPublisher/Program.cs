using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory();

factory.Uri = new Uri("amqps://tpxqyfmf:iNMQ1XlGeXnXelhoQPmath3dDL72moJ8@woodpecker.rmq.cloudamqp.com/tpxqyfmf");

using var connection = factory.CreateConnection();

var channel = connection.CreateModel();

// exclusive true dersem bu kuyruga sadece buradan olusturdugum kanal uzerinden baglanabilirim. Baska bir consumer olusturdugu kanal uzerinden baglanamaz demek. O yuzden false yapiyoruz.
//channel.QueueDeclare("Hello-Queue", true, false, false);

channel.ExchangeDeclare("logs-fanout", type: ExchangeType.Fanout, durable: true);

foreach (var item in Enumerable.Range(1, 50).ToList())
{
    string message = $"log-{item}";

    var messageBody = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish(exchange: "logs-fanout", routingKey: "", basicProperties: null, body: messageBody);

    Console.WriteLine($"Mesaj gönderilmiştir: {message}");
}

Console.ReadLine();