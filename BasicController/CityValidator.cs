using System;
using Lisa.Common.WebApi;

namespace Lisa.Skeleton.Api
{
    public class CityValidator : Validator
    {
        protected override void ValidateModel()
        {
            Required("name");
            NotEmpty("name");
            NotEmpty("country");
        }
    }
}