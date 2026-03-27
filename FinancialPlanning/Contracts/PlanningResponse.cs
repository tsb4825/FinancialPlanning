namespace FinancialPlanning.Contracts
{
    public class PlanningResponse
    {
        public int RetirementAge { get; set; }

        public decimal StartingTotalRetirement { get; set; }

        public decimal EndingTotalRetirement { get; set; }

        public bool DelaySocialSecurityAge { get; set; }

        public IEnumerable<RetirementYearBreakdown> RetirementYearData { get; set; }
    }
}
