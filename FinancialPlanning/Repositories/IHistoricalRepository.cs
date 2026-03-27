using FinancialPlanning.Contracts;

namespace FinancialPlanning.Repositories
{
    public interface IHistoricalRepository
    {
        void SaveData(IEnumerable<Asset> assets, IEnumerable<Liability> liabilities);
        IEnumerable<NetWorthDate> GetHistoricalData();
    }
}
