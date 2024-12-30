using Microsoft.Extensions.Hosting;

namespace SandAndStones.Infrastructure.Contracts
{
    public interface IConsumerService : IHostedService
    {
        void ProcessKafkaMessage(CancellationToken stoppingToken);
    }
}
