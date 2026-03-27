namespace FinancialPlanning.Contracts
{
    public class NetWorthDate
    {
        public DateTime Date { get; set; }
        public IEnumerable<Asset> Assets { get; set; }
        public IEnumerable<Liability> Liabilities { get; set; }
    }
}
