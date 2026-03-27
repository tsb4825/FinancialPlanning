using FinancialPlanning.Contracts;

namespace FinancialPlanning.Repositories
{
    public interface IAssetRepository
    {
        IEnumerable<Asset> GetAssets();

        IEnumerable<Asset> SaveAssets(IEnumerable<Asset> assets);
    }
}
