namespace SandAndStones.Infrastructure.Configuration
{
    public class AzureBlobServiceSettings
    {
        public bool Enabled { get; set; } = false;
        public string StorageAccountName { get; set; } = string.Empty;
        public string StorageAccountKey { get; set; } = string.Empty;
        public string BlobConnectionString { get; set; } = string.Empty;
    }
}
