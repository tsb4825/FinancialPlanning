namespace FinancialPlanning.Contracts
{
    public class RetirementYearBreakdown
    {
        public int RetirementAgeYear { get; set; }

        public decimal StartingInvestmentAmount { get; set; }

        public decimal InvestmentWithdrawlAmount { get; set; }

        public decimal SocialSecurityPayment { get; set; }

        public decimal InvestmentReturnAmount { get; set; }

        public decimal EndingInvestmentAmount { get; set; }
    }
}
