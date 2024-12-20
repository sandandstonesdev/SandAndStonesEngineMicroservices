namespace SandAndStones.Infrastructure.Configuration
{
    public class AzureBlobServiceSettings
    {
        public string StorageAccountName { get; set; }
        public string StorageAccountKey { get; set; }
        public string BlobConnectionString { get; set; }
    }
}
