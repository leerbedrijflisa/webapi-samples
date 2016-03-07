using Lisa.Common.WebApi;
using Microsoft.AspNet.Mvc;
using System.Threading.Tasks;

namespace Lisa.Skeleton.Api
{
    [Route("/cities/")]
    public class CityController : Controller
    {
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var cities = await _database.FetchCitiesAsync();
            return new HttpOkObjectResult(cities);
        }

        [HttpGet("{id}", Name="SingleCity")]
        public async Task<ActionResult> Get(int id)
        {
            var city = await _database.FetchCityAsync(id);
            if (city == null)
            {
                return new HttpNotFoundResult();
            }

            return new HttpOkObjectResult(city);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] DynamicModel city)
        {
            if (city == null)
            {
                return new BadRequestResult();
            }

            var validationResult = _validator.Validate(city);
            if (validationResult.HasErrors)
            {
                return new UnprocessableEntityObjectResult(validationResult.Errors);
            }

            dynamic result = await _database.CreateCityAsync(city);
            string location = Url.RouteUrl("SingleCity", new { id = result.Id }, Request.Scheme);
            return new CreatedResult(location, result);
        }

        private Database _database = new Database();
        private Validator _validator = new CityValidator();
    }
}