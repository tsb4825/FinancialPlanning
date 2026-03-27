using FinancialPlanning.Contracts;

namespace FinancialPlanning.Repositories
{
    public interface IPersonRepository
    {
        PersonData GetPersonData();

        void SavePersonData(PersonData personData);
    }
}
