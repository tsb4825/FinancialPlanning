using FinancialPlanning.Contracts;
using Newtonsoft.Json;

namespace FinancialPlanning.Repositories
{
    public class LiabilitiyRepository : ILiabilityRepository
    {
        private const string FilePath = "LiabilityData.txt";

        public IEnumerable<Liability> GetLiabilities()
        {
            if (File.Exists(FilePath))
            {
                IEnumerable<Liability> liabilities = JsonConvert.DeserializeObject<IEnumerable<Liability>>(File.ReadAllText(FilePath));
                return liabilities;
            }
            
            return Enumerable.Empty<Liability>();
        }

        public IEnumerable<Liability> SaveLiabilities(IEnumerable<Liability> liabilities)
        {
            foreach (var liability in liabilities.Where(x => x.Id == 0))
            {
                liability.Id = liabilities.Max(x => x.Id) + 1;
            }

            File.WriteAllText(FilePath, JsonConvert.SerializeObject(liabilities));

            return liabilities;
        }
    }
}
