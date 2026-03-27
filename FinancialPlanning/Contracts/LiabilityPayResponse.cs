namespace FinancialPlanning.Contracts
{
    public class LoanToPay
    {
        public string Name { get; set; }
        public decimal ExtraPrincipal { get; set; }
        public decimal InterestSaved { get; set; }
        public int PaymentsSaved { get; set; }
        public decimal PaymentAmount { get; set; }
    }
}
