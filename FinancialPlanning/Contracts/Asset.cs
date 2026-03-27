namespace FinancialPlanning.Contracts
{
    public class Asset
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Amount { get; set; }

        public decimal ExpectedReturnRate { get; set; }

        public int CollateralLoanId { get; set; }

        public bool IsInvestment { get; set; }
    }
}
