using FinancialPlanning.Contracts;
using Newtonsoft.Json;

namespace FinancialPlanning.Repositories
{
    public class HistoricalRepository : IHistoricalRepository
    {
        private const string FilePath = "HistoricalData.txt";

        public IEnumerable<NetWorthDate> GetHistoricalData()
        {
            if (File.Exists(FilePath))
            {
                IEnumerable<NetWorthDate> netWorthData = JsonConvert.DeserializeObject<IEnumerable<NetWorthDate>>(File.ReadAllText(FilePath));
                return netWorthData;
            }

            return Enumerable.Empty<NetWorthDate>();
        }

        public void SaveData(IEnumerable<Asset> assets, IEnumerable<Liability> liabilities)
        {
            var historicalData = GetHistoricalData().ToList();
            if (historicalData.FirstOrDefault(x => x.Date == DateTime.Today) == null)
            {
                historicalData.Add(new NetWorthDate { Date = DateTime.Today, Assets = assets, Liabilities = liabilities });
                File.WriteAllText(FilePath, JsonConvert.SerializeObject(historicalData));
            }
        }
    }
}
