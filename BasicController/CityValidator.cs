using Lisa.Common.WebApi;

namespace Lisa.Skeleton.Api
{
    public class CityValidator : Validator
    {
        protected override void ValidateModel()
        {
            Required("name", NotEmpty);
            Optional("country", NotEmpty);
        }

        protected override void ValidatePatch()
        {
            Allow("country");
            Allow("population");
        }
    }
}