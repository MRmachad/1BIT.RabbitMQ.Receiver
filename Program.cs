using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UMBIT.RabbitMQ.Receiver;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

internal class Program
{
    private static void Main(string[] args)
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: "hello",
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        Console.WriteLine(" [*] Waiting for messages.");

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();

            BinaryFormatter bf = new BinaryFormatter();

            using (MemoryStream ms = new MemoryStream(body))
            {
                object obj = bf.Deserialize(ms);
                var o = (Teste) obj;
            }
        };


        channel.BasicConsume(queue: "hello",
                             autoAck: true,
                             consumer: consumer);

        Console.WriteLine(" Press [enter] to exit.");
        Console.ReadLine();
    }
}

