using FinancialPlanning.Contracts;

namespace FinancialPlanning.Repositories
{
    public interface ISpendingRepository
    {
        IEnumerable<MonthlySpend> GetSpending();

        IEnumerable<MonthlySpend> SaveSpending(IEnumerable<MonthlySpend> monthlySpends);
    }
}
