namespace SandAndStones.Infrastructure.Contracts
{
    public interface IProducerService
    {
        Task PublishMessageAsync(string message);
    }
}
