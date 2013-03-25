namespace Indvisible.DataGen.Models
{
    public class ModelWithNested
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Tags { get; set; }

        public ModelWithName Model { get; set; }
    }
}