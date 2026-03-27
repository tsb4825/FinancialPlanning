using FinancialPlanning.Contracts;

namespace FinancialPlanning.Managers
{
    public interface IFinancialPlanningProvider
    {
        IEnumerable<Payment> BuildAmoritizationSchedule(Liability liability);
        IEnumerable<Asset> ProjectInvestmentGrowth(int years, IEnumerable<Asset> assets, decimal yearlyContribution);
        PlanningResponse CalculateRetirementPlan(PlanningData data, IEnumerable<Asset> investments);
        IEnumerable<LoanToPay> CalculateWhichLoansToPayForInterestSaving(decimal extraAmount, IEnumerable<Liability> liabilities);
    }
}
