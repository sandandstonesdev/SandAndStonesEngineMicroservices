namespace SandAndStones.Infrastructure.Contracts
{
    public interface IAzureServiceBusService
    {
        Task SendMessageAsync(string message);
        Task ReceiveMessagesAsync();
    }
}
