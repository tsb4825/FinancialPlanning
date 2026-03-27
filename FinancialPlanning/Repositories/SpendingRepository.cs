using FinancialPlanning.Contracts;
using Newtonsoft.Json;

namespace FinancialPlanning.Repositories
{
    public class SpendingRepository : ISpendingRepository
    {
        private const string FilePath = "SpendingData.txt";

        public IEnumerable<MonthlySpend> GetSpending()
        {
            if (File.Exists(FilePath))
            {
                IEnumerable<MonthlySpend> monthlySpendData = JsonConvert.DeserializeObject<IEnumerable<MonthlySpend>>(File.ReadAllText(FilePath));
                return monthlySpendData;
            }

            return Enumerable.Empty<MonthlySpend>();
        }

        public IEnumerable<MonthlySpend> SaveSpending(IEnumerable<MonthlySpend> monthlySpends)
        {
            foreach (var monthlySpend in monthlySpends.Where(x => x.Id == 0))
            {
                monthlySpend.Id = monthlySpends.Max(x => x.Id) + 1;
            }

            File.WriteAllText(FilePath, JsonConvert.SerializeObject(monthlySpends));

            return monthlySpends;
        }
    }
}
