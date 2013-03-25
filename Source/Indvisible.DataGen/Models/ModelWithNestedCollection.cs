namespace Indvisible.DataGen.Models
{
    using System.Collections.Generic;

    public class ModelWithNestedCollection
    {
        public string Name { get; set; }

        public ICollection<ModelWithName> ModelsWithName { get; set; }
    }
}