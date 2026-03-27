using FinancialPlanning.Contracts;
using Newtonsoft.Json;

namespace FinancialPlanning.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private const string FilePath = "PersonData.txt";

        public PersonData GetPersonData()
        {
            if (File.Exists(FilePath))
            {
                PersonData personData = JsonConvert.DeserializeObject<PersonData>(File.ReadAllText(FilePath));
                return personData;
            }

            return new PersonData();
        }

        public void SavePersonData(PersonData personData)
        {
            File.WriteAllText(FilePath, JsonConvert.SerializeObject(personData));
        }
    }
}
