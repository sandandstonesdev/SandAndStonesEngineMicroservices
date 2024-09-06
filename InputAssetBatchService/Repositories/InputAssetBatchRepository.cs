﻿using SandAndStonesLibrary.AssetConfig;

namespace InputAssetBatchService.Repositories
{
    public class InputAssetBatchRepository : IInputAssetBatchRepository
    {
        public InputAssetBatchRepository() { }
        public async Task<InputAssetBatch> GetById(int id)
        {
            string path = string.Empty;
            if (id == 0)
                path = "./assets.json";
            else if (id == 1)
                path = "./statusBarAssets.json";
                
            var reader = new InputAssetReader(path);
            var batchData = await reader.ReadBatchAsync();

            return batchData ?? new InputAssetBatch(); 
        }
    }
}
