namespace Lisa.Common.WebApi
{
    public abstract class Validator
    {
        public ValidationResult Validate(DynamicModel model)
        {
            Model = model;

            ValidateModel();

            Model = null;
            return Result;
        }

        public ValidationResult Validate(Patch[] patches, DynamicModel model)
        {
            Model = model;

            foreach (var patch in patches)
            {
                _isAllowed = false;
                Patch = patch;
                ValidatePatch();

                if (!_isAllowed)
                {
                    var error = new Error
                    {
                        Code = 684257,
                        Message = $"The field '{patch.Field}' is not patchable.",
                        Values = new
                        {
                            Field = patch.Field
                        }
                    };
                    Result.Errors.Add(error);
                }
            }

            Model = model.Copy();
            ModelPatcher.Apply(patches, Model);
            ValidateModel();

            Model = null;
            Patch = null;
            return Result;
        }

        protected ValidationResult Result { get; private set; } = new ValidationResult();
        protected DynamicModel Model { get; private set; }
        protected Patch Patch { get; private set; }

        protected abstract void ValidateModel();
        protected abstract void ValidatePatch();

        protected void Required(string fieldName)
        {
            if (!Model.ContainsField(fieldName))
            {
                var error = new Error
                {
                    Code = 557385,
                    Message = $"The field '{fieldName}' is required.",
                    Values = new
                    {
                        Field = fieldName
                    }
                };
                Result.Errors.Add(error);
            }
        }

        protected void NotEmpty(string fieldName)
        {
            if (!Model.ContainsField(fieldName))
            {
                return;
            }

            var value = Model.GetValue(fieldName);
            if ((value == null) ||
                (value is string) && (string.IsNullOrWhiteSpace((string) value)))
            {
                var error = new Error
                {
                    Code = 557386,
                    Message = $"The field '{fieldName}' should not be empty.",
                    Values = new
                    {
                        Field = fieldName
                    }
                };
                Result.Errors.Add(error);
            }
        }

        protected void Allow(string fieldName)
        {
            if (Patch.Field == fieldName)
            {
                _isAllowed = true;
            }
        }

        private bool _isAllowed;
    }
}