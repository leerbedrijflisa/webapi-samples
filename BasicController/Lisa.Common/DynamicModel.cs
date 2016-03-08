using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Lisa.Common.WebApi
{
    public class DynamicModel : DynamicObject
    {
        public DynamicModel()
        {
        }

        private DynamicModel(IDictionary<string, object> properties, object metadata)
        {
            _properties = properties;
            _metadata = metadata;
        }

        public DynamicModel Copy()
        {
            // NOTE: The copy and the original now share their metadata. This is probably not how
            // it should be, but how do you copy an arbitrary object?
            var properties = new Dictionary<string, object>(_properties);
            return new DynamicModel(properties, _metadata);
        }

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

        internal IEnumerable<KeyValuePair<string, object>> Properties
        {
            get
            {
                return _properties;
            }
        }

        private object _metadata;
        private IDictionary<string, object> _properties = new Dictionary<string, object>();
    }
}