using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory();

factory.Uri = new Uri("amqps://tpxqyfmf:iNMQ1XlGeXnXelhoQPmath3dDL72moJ8@woodpecker.rmq.cloudamqp.com/tpxqyfmf");

using var connection = factory.CreateConnection();

var channel = connection.CreateModel();

// prefetchsize bana herhangi bir boyuttaki mesaji gonderebilirsin. Herbir consumer a toplam degeri 5 olacak sekilde gonderir. Yani birine 2 adet birine 3 adet.
channel.BasicQos(0, 5, true);

// Herbir consumer a 5 er tane olacak sekilde gonderir.
//channel.BasicQos(0, 5, false);
// Herbir consumer a 1 er tane olacak sekilde gonderir.
channel.BasicQos(0, 1, true);

// Queue declare ile kuyrugu olusturursam kuyruk kalir.
//channel.QueueDeclare("Hello-Queue", true, false, false);

var queueName = channel.QueueDeclare().QueueName;

channel.QueueDeclare(queueName, true, false, false);
channel.QueueBind(queueName, "logs-fanout", "", null);

var consumer = new EventingBasicConsumer(channel);

// Autoack false veriyi dogru islersem ben sana silecegini soyleyecegim. true dersem mesaj bana geldigi gibi mesaji kuyruktan sil.
channel.BasicConsume(queueName, false, consumer);

Console.WriteLine("Loglar dinleniyor...");

consumer.Received += Consumer_Received;

void Consumer_Received(object? sender, BasicDeliverEventArgs e)
{
    Thread.Sleep(1000);

    var message = Encoding.UTF8.GetString(e.Body.ToArray());

    Console.WriteLine("Gelen mesaj:" + message);

    channel.BasicAck(e.DeliveryTag, false);
}

Console.ReadLine();