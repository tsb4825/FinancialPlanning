using FinancialPlanning.Contracts;

namespace FinancialPlanning.Repositories
{
    public interface ILiabilityRepository
    {
        IEnumerable<Liability> GetLiabilities();

        IEnumerable<Liability> SaveLiabilities(IEnumerable<Liability> liabilities);
    }
}
