using FinancialPlanning.Contracts;

namespace FinancialPlanning.Repositories
{
    public interface IPlanningDataRepository
    {
        void SavePlanningData(PlanningData planningData);
        PlanningData GetPlanningData();
    }
}
