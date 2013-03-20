namespace Indvisible.DataGen.Models
{
    public class ModelWithDeep3
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ModelWithDeep2 ModelWithDeep2 { get; set; }
    }
}