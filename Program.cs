using Microsoft.Extensions.Hosting;
using KafkaTestConsumer.Services;
using Microsoft.Extensions.DependencyInjection;

namespace KafkaTestConsumer
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args).ConfigureServices(services =>
            {
                services.AddHostedService<KafkaConsumerService>();
            }).Build();

            await host.RunAsync();
        }
    }
}
