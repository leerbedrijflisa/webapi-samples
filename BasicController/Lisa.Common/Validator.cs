using System;
using System.Collections.Generic;

namespace Lisa.Common.WebApi
{
    public abstract class Validator
    {
        public ValidationResult Validate(DynamicModel model)
        {
            Model = model;

            foreach (var property in model.Properties)
            {
                Property = property;
                ValidateModel();
            }

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
            Validate(Model);    // NOTE: This call sets Model to null at the end. It doesn't
                                // matter, because the next line does the same, but if that ever
                                // changes, you need to fix this.

            Model = null;
            Patch = null;
            return Result;
        }

        protected ValidationResult Result { get; private set; } = new ValidationResult();
        protected DynamicModel Model { get; private set; }
        protected Patch Patch { get; private set; }
        protected KeyValuePair<string, object> Property { get; private set; }

        protected abstract void ValidateModel();
        protected abstract void ValidatePatch();

        protected void Required(string fieldName, params Action<string, object>[] validations)
        {
            if (Property.Key == fieldName)
            {
                foreach (var validation in validations)
                {
                    validation(Property.Key, Property.Value);
                }
            }
        }

        protected void Optional(string fieldName, params Action<string, object>[] validations)
        {
            if (Property.Key == fieldName)
            {
                foreach (var validation in validations)
                {
                    validation(Property.Key, Property.Value);
                }
            }
        }

        protected void NotEmpty(string fieldName, object value)
        {
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