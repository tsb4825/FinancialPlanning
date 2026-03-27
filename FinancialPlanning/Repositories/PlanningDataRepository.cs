using FinancialPlanning.Contracts;
using Newtonsoft.Json;

namespace FinancialPlanning.Repositories
{
    public class PlanningDataRepository : IPlanningDataRepository
    {
        private const string FilePath = "PlanningData.txt";

        public PlanningData GetPlanningData()
        {
            if (File.Exists(FilePath))
            {
                PlanningData planningData = JsonConvert.DeserializeObject<PlanningData>(File.ReadAllText(FilePath));
                return planningData;
            }

            return new PlanningData();
        }

        public void SavePlanningData(PlanningData planningData)
        {
            File.WriteAllText(FilePath, JsonConvert.SerializeObject(planningData));
        }
    }
}
