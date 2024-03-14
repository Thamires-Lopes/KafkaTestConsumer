using Confluent.Kafka;
using KafkaTestConsumer.Models;
using Microsoft.Extensions.Hosting;
using System.Text.Json;

namespace KafkaTestConsumer.Services
{
    public class KafkaConsumerService : BackgroundService
    {
        private readonly IConsumer<Ignore, string> _consumer;
        private readonly ConsumerConfig _consumerConfig;
        private const string BOOTSTRAPSERVER = "localhost:9092";
        private const string GROUP_ID = "Group 1";
        private const string TOPIC_NAME = "usersTopic";

        public KafkaConsumerService()
        {
            _consumerConfig = new ConsumerConfig()
            {
                BootstrapServers = BOOTSTRAPSERVER,
                GroupId = GROUP_ID,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _consumer.Subscribe(TOPIC_NAME);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Run(() =>
                {
                    var result = _consumer.Consume(stoppingToken);
                    var user = JsonSerializer.Deserialize<User>(result.Message.Value);

                    if (user != null)
                    {
                        var text = $"User received - Name: {user.Name} | Id: {user.Id}";
                        Console.WriteLine(text);
                        //Probably I have done something wrong in the package configuration
                        ZipFileHelper.ZipFileHelper.WriteFileWhithText(text);

                    }
                }, stoppingToken);
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _consumer.Close();

            return base.StopAsync(cancellationToken);
        }
    }
}
