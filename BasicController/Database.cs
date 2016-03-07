using Lisa.Common.WebApi;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lisa.Skeleton.Api
{
    // This database class is just a dummy. It stores its data in memory. In a real application,
    // you want to store your data in an actual database, like SQL Server, Table Storage, or
    // RavenDB. In other words, don't copy this code to your own project.
    public class Database
    {
        static Database()
        {
            CreateCities();
        }

        // This function is async even though it doesn't call any async functions (hence the
        // warning from the compiler). Typically, database calls *are* async and this dummy
        // database simulates that, because it influences the way you write controllers.
        public async Task<IEnumerable<DynamicModel>> FetchCitiesAsync()
        {
            return _cities;
        }

        public async Task<DynamicModel> FetchCityAsync(int id)
        {
            foreach (dynamic city in _cities)
            {
                if (city.Id == id)
                {
                    return city;
                }
            }

            return null;
        }

        private static void CreateCities()
        {
            dynamic city = new DynamicModel();
            city.Id = 68;
            city.Name = "Amsterdam";
            city.Country = "The Netherlands";
            city.Population = 1330235;
            _cities.Add(city);

            city = city = new DynamicModel();
            city.Id = 91;
            city.Name = "London";
            city.Country = "United Kingdom";
            city.Population = 9787426;
            _cities.Add(city);

            city = city = new DynamicModel();
            city.Id = 71;
            city.Name = "Athens";
            city.Country = "Greece";
            city.Population = 3090508;
            _cities.Add(city);
        }

        // Cities are stored in a static variable, so that the data is preserved between requests.
        // This makes it easier to test changes made by POST and PATCH request. The data will get
        // reset once you restart the project. Again, in a real application you store your data in
        // a real database, so don't copy this code.
        private static List<DynamicModel> _cities = new List<DynamicModel>();
    }
}