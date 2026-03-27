namespace FinancialPlanning.Contracts
{
    public class Payment
    {
        public decimal Amount { get; set; }

        public decimal Interest { get; set; }

        public decimal Principal { get; set; }

        public decimal RemainingPrincipal { get; set; }
    }
}
