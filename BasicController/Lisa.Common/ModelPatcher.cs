using System.Collections.Generic;

namespace Lisa.Common.WebApi
{
    public class ModelPatcher
    {
        public static void Apply(IEnumerable<Patch> patches, DynamicModel model)
        {
            foreach (var patch in patches)
            {
                model.Replace(patch.Field, patch.Value);
            }
        }
    }
}