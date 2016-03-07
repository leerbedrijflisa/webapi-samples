using Microsoft.AspNet.Mvc;
using System.Threading.Tasks;

namespace Lisa.Skeleton.Api
{
    [Route("/cities/")]
    public class CityController
    {
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var cities = await _database.FetchCitiesAsync();
            return new HttpOkObjectResult(cities);
        }

        private Database _database = new Database();
    }
}