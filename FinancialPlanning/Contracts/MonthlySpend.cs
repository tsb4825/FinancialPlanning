namespace FinancialPlanning.Contracts
{
    public class MonthlySpend
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }

        public int? LiabilityId { get; set; }

        public string Name { get; set; }
    }
}
