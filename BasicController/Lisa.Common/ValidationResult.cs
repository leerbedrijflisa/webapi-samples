using System.Collections.Generic;

namespace Lisa.Common.WebApi
{
    public class ValidationResult
    {
        public ValidationResult()
        {
            Errors = new List<Error>();
        }

        public IList<Error> Errors { get; private set; }
        public bool HasErrors
        {
            get
            {
                return Errors.Count > 0;
            }
        }
    }
}