namespace Indvisible.DataGen.Models
{
    using System.Collections.Generic;

    public class ModelWithNestedEnumerable
    {
        public string Name { get; set; }

        public IEnumerable<ModelWithName> ModelsWithName { get; set; }
    }
}