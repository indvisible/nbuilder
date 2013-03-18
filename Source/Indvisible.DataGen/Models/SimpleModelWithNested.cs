namespace Indvisible.DataGen.Models
{
    using System.Collections.Generic;

    public class SimpleModelWithNested
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Tags { get; set; }

        public SimpleModelWithName SimpleModel { get; set; }
    }
}