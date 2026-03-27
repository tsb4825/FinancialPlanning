using FinancialPlanning.Contracts.Enums;

namespace FinancialPlanning.Contracts
{
    public class Liability
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Principal { get; set; }

        public decimal OriginalPrincipal { get; set; }

        public decimal InterestRate { get; set; }

        public decimal FrequencyPayment { get; set; }

        public FrequencyPaymentType FrequencyPaymentType { get; set; }

        public decimal EscrowMonthlyAmount { get; set; }

        public string ProjectedPayoff { get; set; }
    }
}
