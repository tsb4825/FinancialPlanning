using FinancialPlanning.Contracts;
using Newtonsoft.Json;

namespace FinancialPlanning.Repositories
{
    public class AssetRepository : IAssetRepository
    {
        private const string FilePath = "AssetData.txt";

        public IEnumerable<Asset> GetAssets()
        {
            if (File.Exists(FilePath))
            {
                IEnumerable<Asset> assetData = JsonConvert.DeserializeObject<IEnumerable<Asset>>(File.ReadAllText(FilePath));
                return assetData;
            }
            
            return Enumerable.Empty<Asset>();
        }

        public IEnumerable<Asset> SaveAssets(IEnumerable<Asset> assets)
        {
            foreach(var asset in assets.Where(x => x.Id == 0))
            {
                asset.Id = assets.Max(x => x.Id) + 1;
            }

            File.WriteAllText(FilePath, JsonConvert.SerializeObject(assets));

            return assets;
        }
    }
}
