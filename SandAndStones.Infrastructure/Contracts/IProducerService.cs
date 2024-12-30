namespace SandAndStones.Infrastructure.Contracts
{
    public interface IProducerService
    {
        Task ProduceAsync(string message);
    }
}
