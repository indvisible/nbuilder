namespace Indvisible.DataGen.Models
{
    using System.Collections.Generic;

    public class ModelWithNestedIList
    {
        public string Name { get; set; }

        public IList<ModelWithName> ModelsWithName { get; set; }
    }
}