using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Lisa.Common.WebApi
{
    public class DynamicModel : DynamicObject
    {
        public object GetMetadata()
        {
            return _metadata;
        }

        public void SetMetadata(object metadata)
        {
            _metadata = metadata;
        }

        public bool ContainsField(string fieldName)
        {
            string normalizedName = fieldName.ToLowerInvariant();
            return _properties.ContainsKey(normalizedName);
        }

        public object GetValue(string fieldName)
        {
            string normalizedName = fieldName.ToLowerInvariant();
            if (!_properties.ContainsKey(normalizedName))
            {
                throw new InvalidOperationException($"The field '{fieldName}' doesn't exist.");
            }

            return _properties[normalizedName];
        }

        public void Replace(string fieldName, object value)
        {
            string normalizedName = fieldName.ToLowerInvariant();
            if (!_properties.ContainsKey(normalizedName))
            {
                throw new InvalidOperationException($"The field '{fieldName}' doesn't exist.");
            }

            _properties[normalizedName] = value;
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return _properties.Keys;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            // TODO: Only make the name case-insensitive when binder indicates it.
            string normalizedName = binder.Name.ToLowerInvariant();
            if (!_properties.ContainsKey(normalizedName))
            {
                return base.TryGetMember(binder, out result);
            }

            result = _properties[normalizedName];
            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            // TODO: Only make the name case-insensitive when binder indicates it.
            string normalizedName = binder.Name.ToLowerInvariant();
            _properties[normalizedName] = value;

            return true;
        }

        private object _metadata;
        private IDictionary<string, object> _properties = new Dictionary<string, object>();
    }
}